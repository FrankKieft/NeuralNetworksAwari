using System;
using System.Collections.Generic;

namespace NeuralNetworksAwari.AwariEngine.NeuralNetworks.Interfaces
{
    public interface IOutputNeuron: INeuron
    {
        Dictionary<Guid, double> WeightingFactors { get; }

        void AcceptSignal(INeuron[] neuron);
    }
}