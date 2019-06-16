using NeuralNetworksAwari.AwariEngine.NeuralNetworks.Interfaces;

namespace NeuralNetworksAwari.AwariEngine.NeuralNetworks
{
    public class LearningOutputNeuron : OutputNeuron, ILearning
    {
        private  ILearning[] _previousNeuronLayer;

        public LearningOutputNeuron(int index, double[] weightingFactors, ILearning[] previousNeuronLayer):base(index,weightingFactors)
        {
            _previousNeuronLayer = previousNeuronLayer;
        }

        public void Learn(double factor)
        {
            foreach(var n in _previousNeuronLayer)
            {                
                if (n.Value>0)
                {
                    n.Learn(factor);
                    WeightingFactors[n.Index] += factor * n.Value;
                }
                else
                {
                    n.Learn(-factor);
                    WeightingFactors[n.Index] *= 1 - factor;
                }
            }
        }
    }
}
