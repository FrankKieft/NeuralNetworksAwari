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

        private List<EvaluationResult> EvaluateParallel(AwariPosition position, int depth)
        {
            var evaluations = new List<Evaluation>();
            var results = new List<EvaluationResult>();

            var first = position.CanSow();

            if (first.Count == 0)
            {
                results.Add(new EvaluationResult(position.Position[AwariPosition.SOUTH_AWARI] - position.Position[AwariPosition.NORTH_AWARI]));
            }
            else
            {
                foreach (var f in first)
                {
                    var p1 = position.Copy();
                    p1.Sow(f);

                    var second = p1.CanSow();

                    if (second.Count == 0)
                    {
                        results.Add(new EvaluationResult(p1.Position[AwariPosition.NORTH_AWARI] - p1.Position[AwariPosition.SOUTH_AWARI], f));
                        continue;
                    }
                    foreach (var s in second)
                    {
                        var p2 = position.Copy();
                        p2.Sow(f);
                        p2.Sow(s);
                        evaluations.Add(new Evaluation(position: p2, firstMove: f, secondMove: s));
                    }
                }
            }

            Parallel.ForEach(evaluations, x => {
                results.Add(
                         new EvaluationResult(
                             value: EvaluateRecursive(x.Position, depth - 2),
                             firstMove: x.FirstMove,
                             secondMove: x.SecondMove));
            });


            return results;
        }

        public override int Evaluate(AwariPosition position, int depth)
        {            
            var results = EvaluateParallel(position, depth);

            return results.GroupBy(x => x.FirstMove)
                .Select(x => new EvaluationResult(value: x.Min(y => y.Value), firstMove: x.Key))
                .OrderByDescending(x => x.Value)
                .First()
                .Value;
        }
    }
}