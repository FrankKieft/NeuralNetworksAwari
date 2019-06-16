namespace NeuralNetworksAwari.AwariEngine.NeuralNetworks.Interfaces
{
    public interface IInput: INeuron
    {
        void AcceptAwariPits(int[] pits);
    }
}