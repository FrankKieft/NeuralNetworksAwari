namespace NeuralNetworksAwari.AwariEngine.NeuralNetworks.Interfaces
{
    public interface INeuron
    {
        int Index { get; }
        double Value { get; }
        double[] WeightingFactors { get; }
    }
}