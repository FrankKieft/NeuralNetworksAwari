using System;

namespace NeuralNetworksAwari.AwariEngine
{
    public class AwariEvaluator
    {
        public AwariEvaluator()
        {
        }
        
        public int Evaluate(AwariPosition position, int depth, int factor = 1)
        {
            Func<int> score = () => factor * (position.Position[AwariPosition.SOUTH_AWARI] - position.Position[AwariPosition.NORTH_AWARI]);

            if (depth==0)
            {
                return score();
            }

            var possibleMoves = position.CanSow();
            if (possibleMoves.Count == 0)
            {
                return score();
            }

            var finalEvaluationResult = factor * -1000;
            foreach (var m in possibleMoves)
            {
                position.Sow(m);
                var evaluationResult = Evaluate(position, depth - 1, factor * -1);
                if (factor == 1)
                {
                    if (evaluationResult > finalEvaluationResult)
                    {
                        finalEvaluationResult = evaluationResult;
                    }
                }
                else if (evaluationResult < finalEvaluationResult)
                {
                    finalEvaluationResult = evaluationResult;
                }

                position.MoveBack();
            }

            return finalEvaluationResult;
        }
    }
}