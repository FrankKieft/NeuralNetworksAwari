using System;

namespace NeuralNetworksAwari.AwariEngine.NeuralNetworks.Interfaces
{
    public interface IOutputNeuron: INeuron
    {
        void Subscribe(Guid key);
        void AcceptSignal(INeuron neuron);
        void Reset();
    }
}