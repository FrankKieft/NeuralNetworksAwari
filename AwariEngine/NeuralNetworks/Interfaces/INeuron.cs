using System;

namespace NeuralNetworksAwari.AwariEngine.NeuralNetworks.Interfaces
{
    public interface INeuron
    {
        Guid Key { get; }
        double Value { get; }
        void SendSignal();
    }
}