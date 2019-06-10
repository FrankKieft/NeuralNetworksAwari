using NeuralNetworksAwari.AwariEngine.NeuralNetwork.Interfaces;
using System;
using System.Collections.Generic;

namespace NeuralNetworksAwari.AwariEngine.NeuralNetwork
{
    public class OutputNeuron : IOutputNeuron
    {
        private IRandomizer _random;
        private Dictionary<Guid, double> _weightingFactors;
        private List<IOutputNeuron> _neurons;

        public OutputNeuron(List<IOutputNeuron> neurons, IRandomizer random): this(random)
        {
            _neurons = neurons;
            _neurons.ForEach(x => x.Subscribe(Key));
        }

        public OutputNeuron(IRandomizer random)
        {
            Key = Guid.NewGuid();
            _random = random;
            _weightingFactors = new Dictionary<Guid, double>();            
        }

        public void Subscribe(Guid key)
        {
            _weightingFactors.Add(key, _random.GetDouble());
        }

        public void AcceptSignal(INeuron neuron)
        {
            Value += _weightingFactors[neuron.Key] * neuron.Value;
        }

        public void Reset()
        {
            Value = 0;
        }

        public void SendSignal()
        {
            _neurons.ForEach(x => x.AcceptSignal(this));
        }

        public double Value { get; private set; }
        public Guid Key { get; }
    }
}
