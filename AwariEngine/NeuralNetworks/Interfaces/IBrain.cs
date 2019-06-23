namespace NeuralNetworksAwari.AwariEngine.NeuralNetworks.Interfaces
{
    public interface IBrain
    {
        void BuildNeuronLayers();
        IOutput[] Evaluate(int[] pits, int capturedStones);
        void Learn(int[] pits, int capturedStones, int score);
        void StoreWeightFactors();
    }
}