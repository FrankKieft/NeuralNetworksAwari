namespace NeuralNetworksAwari.AwariEngine.Util
{
    public interface IRandomizer
    {
        double GetDouble();
        int Next(int maxValue);
        int Next(int minValue, int maxValue);
    }
}