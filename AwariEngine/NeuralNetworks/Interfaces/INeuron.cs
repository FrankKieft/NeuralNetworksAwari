namespace NeuralNetworksAwari.AwariEngine.NeuralNetworks.Interfaces
{
    public interface INeuron
    {
        int Index { get; }
        double[] WeightingFactors { get; }
        void Learn(double factor);
    }
}