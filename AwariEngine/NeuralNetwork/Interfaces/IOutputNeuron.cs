using System;

namespace NeuralNetworksAwari.AwariEngine.NeuralNetwork.Interfaces
{
    public interface IOutputNeuron: INeuron
    {
        void Subscribe(Guid key);
        void AcceptSignal(INeuron neuron);
        void Reset();
    }
}