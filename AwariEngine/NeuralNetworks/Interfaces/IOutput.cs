namespace NeuralNetworksAwari.AwariEngine.NeuralNetworks.Interfaces
{
    public interface IOutput: INeuron
    {
        void AcceptSignal(INeuron[] neuron);
    }
}