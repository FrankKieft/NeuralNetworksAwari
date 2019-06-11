using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NeuralNetworksAwari.AwariEngine.Evaluations
{
    public class ParallelAwariEvaluator : AwariEvaluator
    {
        public ParallelAwariEvaluator()
        {
        }

        public override int Evaluate(AwariPosition position, int depth)
        {
            var evaluations = new List<Evaluation>();

            var canSowFirst = position.CanSow();
            if (canSowFirst.Count == 0)
            {
                return position.Position[AwariPosition.SOUTH_AWARI] - position.Position[AwariPosition.NORTH_AWARI];
            }

            foreach (var fm in canSowFirst)
            {
                var p1 = position.Copy();
                p1.Sow(fm);
                var canSowSecond = p1.CanSow();
                if (canSowSecond.Count == 0)
                {
                    evaluations.Add(
                        new Evaluation
                        {
                            FirstMove = fm,
                            Value = position.Position[AwariPosition.NORTH_AWARI] - position.Position[AwariPosition.SOUTH_AWARI]
                        });
                }
                else
                {
                    foreach (var sm in canSowSecond)
                    {
                        var p2 = position.Copy();
                        p2.Sow(sm);
                        evaluations.Add(new Evaluation
                        {
                            Position=p2,
                            FirstMove=fm,
                            SecondMove=sm
                        });
                    }
                }
            }
                               
            Parallel.ForEach(evaluations, eval =>
            {
                eval.Value = base.Evaluate(eval.Position, depth - 2);
            });

            return evaluations.GroupBy(x => x.FirstMove)
                .Select(x => new Evaluation { FirstMove = x.Key, Value = x.Min(y => y.Value) })
                .OrderByDescending(x => x.Value)
                .First()
                .Value;
        }

    }
}