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
    }
}
