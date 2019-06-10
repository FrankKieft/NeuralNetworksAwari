using System;

namespace NeuralNetworksAwari.AwariEngine.NeuralNetwork.Interfaces
{
    public interface INeuron
    {
        Guid Key { get; }
        double Value { get; }
        void SendSignal();
    }
}