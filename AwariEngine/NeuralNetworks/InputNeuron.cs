using NeuralNetworksAwari.AwariEngine.NeuralNetworks.Interfaces;
using System;

namespace NeuralNetworksAwari.AwariEngine.NeuralNetworks
{
    public class InputNeuron : IInputNeuron
    {
        public InputNeuron(Guid key, double[] weightingFactors)
        {
            WeightingFactors = weightingFactors;
        }

        public Guid Key { get; }
        public double Value { get; private set; }
        public double[] WeightingFactors { get; private set; }

        public void AcceptAwariPits(int[] pits)
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
