using NeuralNetworksAwari.AwariEngine.NeuralNetworks.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace NeuralNetworksAwari.AwariEngine.NeuralNetworks
{
    public class LearningOutputNeuron : INeuron
    {
        private  List<ILearningOutputNeuron> _previousNeuronLayer;
        private readonly IOutputNeuron _neuron;

        public LearningOutputNeuron(IOutputNeuron neuron)
        {
            _neuron = neuron;
        }

        public double Value { get { return _neuron.Value; } }
        public Guid Key { get { return _neuron.Key; } }
        public Dictionary<Guid, double> WeightingFactors { get { return _neuron.WeightingFactors; } }
        
        public void AcceptSignal(ILearningOutputNeuron[] neurons)
        {
            _previousNeuronLayer = neurons.ToList();
            _neuron.AcceptSignal(neurons);
        }

        public void Learn(double factor)
        {
            foreach(var n in _previousNeuronLayer)
            {                
                if (n.Value>0)
                {
                    n.Learn(factor);
                    WeightingFactors[n.Key] += factor * n.Value;
                }
                else
                {
                    n.Learn(-factor);
                    WeightingFactors[n.Key] *= 1 - factor;
                }
            }
        }
    }
}
