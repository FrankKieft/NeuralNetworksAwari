using NeuralNetworksAwari.AwariEngine.NeuralNetworks.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace NeuralNetworksAwari.AwariEngine.NeuralNetworks
{
    public class InputNeuron : INeuron
    {
        private List<IOutputNeuron> _neurons;
        private double[] _weightingFactors;

        public InputNeuron(List<IOutputNeuron> neurons, IRandomizer random)
        {
            Key = Guid.NewGuid();
            _neurons = neurons;
            _neurons.ForEach(x => x.Subscribe(Key));
            _weightingFactors = new double[572];
            Enumerable.Range(0, 572).ToList().ForEach(x => _weightingFactors[x]=random.GetDouble());
        }

        public Guid Key { get; }
        public double Value { get; private set; }

        public void Accept(int[] pits)
        {
            for(var i=0; i<572; i++)
            {
                var pit = i / 48;
                var stones = i % 48;
                if (pits[pit] > stones)
                {
                    Value += _weightingFactors[i];
                }
            }
        }

        public void SendSignal()
        {
            _neurons.ForEach(x => x.AcceptSignal(this));
        }
    }
}
