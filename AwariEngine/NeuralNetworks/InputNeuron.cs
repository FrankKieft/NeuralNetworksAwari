using NeuralNetworksAwari.AwariEngine.NeuralNetworks.Interfaces;

namespace NeuralNetworksAwari.AwariEngine.NeuralNetworks
{
    public class InputNeuron : INeuron, IInput
    {
        public InputNeuron(int index, double[] weightingFactors)
        {
            Index = index;
            WeightingFactors = weightingFactors;
        }

        public int Index { get;  }
        public double Value { get; private set; }
        public double[] WeightingFactors { get; }
        
        public virtual void AcceptAwariPits(int[] pits)
        {
            for(var i=0; i<572; i++)
            {
                var pit = i / 48;
                var stones = i % 48;
                if (pits[pit] > stones)
                {
                    Value += WeightingFactors[i];
                }
            }
            Value = Value / 48;
        }
    }
}
