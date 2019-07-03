using NeuralNetworksAwari.AwariEngine.Evaluations;
using NeuralNetworksAwari.AwariEngine.NeuralNetworks.Interfaces;
using NeuralNetworksAwari.AwariEngine.Util;
using System;

namespace NeuralNetworksAwari.AwariEngine.NeuralNetworks
{
    public class Teacher
    {
        private readonly AwariEvaluator _evaluator;
        private IRandomizer _randomizer;

        public Teacher(IRandomizer randomizer, AwariEvaluator evaluator)
        {
            _evaluator = evaluator;
            _randomizer = randomizer;
        }

        public void Teach(IBrain brain, int iterations)
        {
            var analyser = new Analyser(brain);

            var depth = 4;

            for (var i = 0; i < iterations; i++)
            {
                var stones = _randomizer.Next(1, 48);
                var position = GetAwariPosition(stones);
                var p = position.GetPits();

                if (p[0] > 0 || p[1] > 0 || p[2] > 0 || p[3] > 0 || p[4] > 0 || p[5] > 0 )
                {
                    analyser.Start(i, p, 48 - stones);

                    var score = _evaluator.Evaluate(position, 5)+48-stones;
                    brain.Learn(p, 48 - stones, score);

                    analyser.Stop(score);
                }
            }

            analyser.Dispose();
        }

        private AwariPosition GetAwariPosition(int stones)
        {
            var position = new int[14];

            for (var i=0; i<stones; i++)
            {
                position[_randomizer.Next(12)]++;
            }
            position[13] = 48 - stones;

            return new AwariPosition(position);
        }
    }
}