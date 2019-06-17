using NeuralNetworksAwari.AwariEngine.NeuralNetworks;
using NeuralNetworksAwari.AwariEngine.NeuralNetworks.Interfaces;
using System;
using System.Linq;

namespace NeuralNetworksAwari.AwariEngineTests.NeuralNetworks
{
    public class Brain
    {
        private const int AWARI_PARAMETERS = 12 * 47 + 48;
        private const int INPUT_NEURONS = 6 * 48;
        private const int INTERMEDIATE_NEURONS = 4 * 48;
        private const int OUTPUT_NEURONS = 2 * 48 + 1;

        private const double FACTOR = 0.01d;

        private IRandomizer _randomizer;

        private readonly InputNeuron[] _inputs;
        private readonly IntermediateNeuron[] _intermediates;
        private readonly OutputNeuron[] _outputs;

        public Brain(IRandomizer randomizer)
        {
            _randomizer = randomizer;

            _inputs = Enumerable.Range(0, INPUT_NEURONS).ToList()
                .Select(x => new InputNeuron(x, GetWeightingFactors(AWARI_PARAMETERS))).ToArray();

            _intermediates = Enumerable.Range(0, INTERMEDIATE_NEURONS).ToList()
                .Select(x => new IntermediateNeuron(x, GetWeightingFactors(INPUT_NEURONS), _inputs)).ToArray();

            _outputs = Enumerable.Range(0, OUTPUT_NEURONS).ToList()
                .Select(x => new OutputNeuron(x, GetWeightingFactors(INTERMEDIATE_NEURONS), _intermediates)).ToArray();
        }

        public IOutput[] Evaluate(int[] pits, int capturedStones)
        {
            for (var i=0; i<_inputs.Length; i++)
            {
                _inputs[i].AcceptAwariPits(pits, capturedStones);
            }

            for (var i = 0; i < _intermediates.Length; i++)
            {
                _intermediates[i].AcceptSignal();
            }

            for (var i = 0; i < _outputs.Length; i++)
            {
                _outputs[i].AcceptSignal();
            }

            return _outputs;
        }

        public void Learn(int[] pits, int capturedStones, int score)
        {
            var scores = Evaluate(pits, capturedStones);

            var highScore = scores.ToList().OrderByDescending(x => x.Value).First();

            if (highScore.Index != 48+score)
            {
                highScore.Learn(-FACTOR);
            }

            scores[48 + score].Learn(FACTOR);
        }

        private double[] GetWeightingFactors(int n)
        {
            var factors = new double[n];
            for(var i=0; i<factors.Length; i++)
            {
                factors[i] = _randomizer.GetDouble();
            }
            return factors;
        }
    }
}