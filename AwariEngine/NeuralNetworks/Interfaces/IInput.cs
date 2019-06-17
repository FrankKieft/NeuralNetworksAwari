namespace NeuralNetworksAwari.AwariEngine.NeuralNetworks.Interfaces
{
    public interface IInput: ISender
    {
        void AcceptAwariPits(int[] pits, int totalPits);
    }
}