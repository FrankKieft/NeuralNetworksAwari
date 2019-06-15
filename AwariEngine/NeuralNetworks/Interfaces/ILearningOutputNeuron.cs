namespace NeuralNetworksAwari.AwariEngine.NeuralNetworks.Interfaces
{
    public interface ILearningOutputNeuron: INeuron
    {
        void Learn(double factor);
        void AcceptSignal(ILearningOutputNeuron[] neuron);
    }
}