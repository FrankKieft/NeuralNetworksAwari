using System;
using NeuralNetworksAwari.AwariEngine.NeuralNetwork.Interfaces;

namespace NeuralNetworksAwari.AwariEngine.NeuralNetwork
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
