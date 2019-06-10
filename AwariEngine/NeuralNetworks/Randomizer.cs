using System;
using NeuralNetworksAwari.AwariEngine.NeuralNetworks.Interfaces;

namespace NeuralNetworksAwari.AwariEngine.NeuralNetworks
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
