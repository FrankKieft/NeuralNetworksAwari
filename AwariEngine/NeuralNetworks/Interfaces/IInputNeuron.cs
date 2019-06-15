namespace NeuralNetworksAwari.AwariEngine.NeuralNetworks.Interfaces
{
    public interface IInputNeuron: INeuron
    {
        double[] WeightingFactors { get; }

        void AcceptAwariPits(int[] pits);
    }
}