﻿namespace NeuralNetworksAwari.AwariEngine.NeuralNetworks.Interfaces
{
    public interface IOutput : IReceiver
    {
        double Value { get; }
    }
}