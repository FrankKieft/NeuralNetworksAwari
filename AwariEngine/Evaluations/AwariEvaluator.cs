using System;

namespace NeuralNetworksAwari.AwariEngine.Evaluations
{
    public class AwariEvaluator
    {
        public AwariEvaluator()
        {
        }
        
        public virtual int Evaluate(AwariPosition position, int depth)
        {
            return EvaluateRecursive(position, depth);
        }

        protected int EvaluateRecursive(AwariPosition position, int depth)
        {
            if (depth==0)
            {
                return position.Position[AwariPosition.SOUTH_AWARI] - position.Position[AwariPosition.NORTH_AWARI];
            }

            var moves = position.CanSow();
            if (moves.Count==0)
            {
                return position.Position[AwariPosition.SOUTH_AWARI] - position.Position[AwariPosition.NORTH_AWARI];
            }

            var score = -1000;
            foreach(var move in moves)
            {
                position.Sow(move);

                var eval = -EvaluateRecursive(position, depth - 1);

                if (eval > score)
                {
                    score = eval;
                }

                position.MoveBack();
            }

            return score;
        }
    }
}