using NeuralNetworksAwari.AwariEngine.NeuralNetworks.Interfaces;
using System;
using System.Collections.Generic;

namespace NeuralNetworksAwari.AwariEngine.NeuralNetworks
{
    public class OutputNeuron : IOutputNeuron
    {
        public OutputNeuron(Guid key, Dictionary<Guid, double> weightingFactors)
        {
            Key = key;
            WeightingFactors = weightingFactors;
        }
        
        public double Value { get; set; }
        public Guid Key { get; }
        public Dictionary<Guid, double> WeightingFactors { get; private set; }

        public void AcceptSignal(INeuron neuron)
        {
            Value += WeightingFactors[neuron.Key] * neuron.Value;
        }
    }
}
