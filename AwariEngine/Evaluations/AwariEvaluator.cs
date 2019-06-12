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

            var finalEvaluationResult = -1000;
            var score = 0;
            for(var i= 0; i<6; i++)
            {
                if (position.Position[i] == 0)
                {
                    continue;
                }
                position.Sow(i);

                var evaluationResult = -EvaluateRecursive(position, depth - 1);
                if (evaluationResult > finalEvaluationResult)
                {
                    if (evaluationResult == 1000)
                    {
                        if (finalEvaluationResult < -800)
                        {
                            if (finalEvaluationResult < -900)
                            {
                                finalEvaluationResult = -900;
                                score = position.Position[AwariPosition.NORTH_AWARI]-position.Position[AwariPosition.SOUTH_AWARI];
                            }
                            else
                            {
                                if (score < position.Position[AwariPosition.SOUTH_AWARI] - position.Position[AwariPosition.NORTH_AWARI])
                                    score = position.Position[AwariPosition.SOUTH_AWARI] - position.Position[AwariPosition.NORTH_AWARI];
                            }
                        }
                    }                    
                    else
                    {
                        finalEvaluationResult = evaluationResult;
                    }
                }
                
                position.MoveBack();
            }

            return finalEvaluationResult == -900 ? score: finalEvaluationResult;
        }
    }
}