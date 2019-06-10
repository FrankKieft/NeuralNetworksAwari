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

                var evaluationResult = -Evaluate(position, depth - 1);
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