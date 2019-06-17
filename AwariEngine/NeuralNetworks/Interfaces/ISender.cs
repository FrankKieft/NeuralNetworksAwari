namespace NeuralNetworksAwari.AwariEngine.NeuralNetworks.Interfaces
{
    public interface ISender : INeuron
    {
        bool Signal { get; }
    }
}