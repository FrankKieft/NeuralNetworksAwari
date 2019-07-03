using System;

namespace NeuralNetworksAwari.AwariEngine.Util
{
    public class Randomizer : IRandomizer
    {
        private Random _random;

        public Randomizer()
        {
            _random = new Random((int)DateTime.Now.Ticks);
        }

        public double GetDouble()
        {
            return _random.NextDouble();
        }

        public int Next(int maxValue)
        {
            return _random.Next(maxValue);
        }

        public int Next(int minValue, int maxValue)
        {
            return _random.Next(minValue, maxValue);
        }
    }
}
