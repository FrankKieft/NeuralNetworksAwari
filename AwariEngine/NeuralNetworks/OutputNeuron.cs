using NeuralNetworksAwari.AwariEngine.NeuralNetworks.Interfaces;

namespace NeuralNetworksAwari.AwariEngine.NeuralNetworks
{
    public class OutputNeuron : INeuron, IOutput
    {
        public OutputNeuron(int index, double[] weightingFactors)
        {
            Index=index;
            WeightingFactors = weightingFactors;
        }
        
        public double Value { get; protected set; }
        public int Index { get; }
        public double[] WeightingFactors { get; }

        public void AcceptSignal(INeuron[] neurons)
        {
            Value = 0;
            for (var i = 0; i < neurons.Length; i++)
            {
                Value += neurons[i].Value * WeightingFactors[neurons[i].Index];
            }
        }
    }
}
