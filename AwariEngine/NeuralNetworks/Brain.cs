using NeuralNetworksAwari.AwariEngine.NeuralNetworks;
using NeuralNetworksAwari.AwariEngine.NeuralNetworks.Interfaces;
using NeuralNetworksAwari.AwariEngine.Util;
using System;
using System.IO;
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
        private readonly IFactorRepository repository;
        private readonly InputNeuron[] _inputs;
        private readonly IntermediateNeuron[] _intermediates;
        private readonly OutputNeuron[] _outputs;

        public Brain(IRandomizer randomizer, IFactorRepository repository)
        {
            _randomizer = randomizer;
            this.repository = repository;
            _inputs = Enumerable.Range(0, INPUT_NEURONS).ToList()
                .Select(x => new InputNeuron(x, GetWeightingFactors(AWARI_PARAMETERS))).ToArray();

            _intermediates = Enumerable.Range(0, INTERMEDIATE_NEURONS).ToList()
                .Select(x => new IntermediateNeuron(x, GetWeightingFactors(INPUT_NEURONS), _inputs)).ToArray();

            _outputs = Enumerable.Range(0, OUTPUT_NEURONS).ToList()
                .Select(x => new OutputNeuron(x, GetWeightingFactors(INTERMEDIATE_NEURONS), _intermediates)).ToArray();
        }

        /// <summary>
        /// Likelyness of each possible outcome. 
        /// </summary>
        /// <param name="pits">Values of 12 pits</param>
        /// <param name="capturedStones">Stones not in pit to make the total 48</param>
        /// <returns>97 Output neurons. Each neuron representing a possible score. Index 0 is -48 until 97 is +48. Index 48 being equality or 0.</returns>
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

            var highScores = scores.ToList().OrderByDescending(x => x.Value).ToList();
            var i = 0;
            while ((i == 0 || highScores[i].Value >= 1d) && i != score + 48 && highScores[i].Value > 0d)
            {
                highScores[i++].Learn(-FACTOR);

            }

            if (scores[48 + score].Value < 1d)
            {
                scores[48 + score].Learn(FACTOR);
            }
        }
        
        public void StoreWeightFactors()
        {
            StoreWeightFactors("input", _inputs);
            StoreWeightFactors("intermediate", _inputs);
            StoreWeightFactors("output", _inputs);
        }

        public void StoreWeightFactors(string name, INeuron[] neurons)
        {
            var directory = $"{Environment.CurrentDirectory}\\factors";

            if (!Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }
            
            for (var i=0; i< neurons.Length; i++)
            {
                using (var sw = new StreamWriter($"{directory}\\{name}{i}"))
                {
                    foreach (var f in neurons[i].WeightingFactors)
                        sw.WriteLine($"{f}");
                }
            }
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