using System;

namespace NeuralNetworksAwari.AwariEngine.NeuralNetworks
{
    public struct NeuronSignalValue
    {
        public NeuronSignalValue(Guid key, double value, double weightingFactor)
        {
            Key = key;
            Value = value * weightingFactor;
        }

        public Guid Key { get; }
        public double Value { get; }
    }
}
