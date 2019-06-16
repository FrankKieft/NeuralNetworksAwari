namespace NeuralNetworksAwari.AwariEngine.NeuralNetworks.Interfaces
{
    public interface ILearning: INeuron
    {
        void Learn(double factor);
    }
}