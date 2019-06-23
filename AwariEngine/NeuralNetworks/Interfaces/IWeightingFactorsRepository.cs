namespace NeuralNetworksAwari.AwariEngine.NeuralNetworks.Interfaces
{
    public interface IWeightingFactorsRepository
    {
        bool HasWeightingFactors { get; }

        void Store(string name, INeuron[] neurons);
        double[] Read(string name, int i);
        double[] Create(int n);
        void DeleteAll();
    }
}