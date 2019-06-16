using NeuralNetworksAwari.AwariEngine.NeuralNetworks;
using NeuralNetworksAwari.AwariEngine.NeuralNetworks.Interfaces;
using System;
using System.Linq;

namespace NeuralNetworksAwari.AwariEngineTests.NeuralNetworks
{
    public class Brain
    {
        private const int AWARI_PARAMETERS = 12 * 48;
        private const int INPUT_NEURONS = 194;
        private const int FIRST_LAYER_NEURONS = 194;
        private const int SECOND_LAYER_NEURONS = 194;
        private const int OUTPUT_NEURONS = 2 * 48 + 1;


        private readonly InputNeuron[] _inputNeurons;
        private IRandomizer _randomizer;
        private readonly OutputNeuron[] _firstLayerIntermediateNeurons;
        private readonly OutputNeuron[] _secondLayerIntermediateNeurons;
        private OutputNeuron[] _outputNeurons;

        public Brain(IRandomizer randomizer)
        {
            _randomizer = randomizer;

            _inputNeurons = Enumerable.Range(0, INPUT_NEURONS).ToList()
                .Select(x => new InputNeuron(x, GetWeightingFactors(AWARI_PARAMETERS))).ToArray();

            _firstLayerIntermediateNeurons = Enumerable.Range(0, FIRST_LAYER_NEURONS).ToList()
                .Select(x => new OutputNeuron(x, GetWeightingFactors(INPUT_NEURONS))).ToArray();

            _secondLayerIntermediateNeurons = Enumerable.Range(0, SECOND_LAYER_NEURONS).ToList()
                .Select(x => new OutputNeuron(x, GetWeightingFactors(FIRST_LAYER_NEURONS))).ToArray();

            _outputNeurons = Enumerable.Range(0, OUTPUT_NEURONS).ToList()
                .Select(x => new OutputNeuron(x, GetWeightingFactors(SECOND_LAYER_NEURONS))).ToArray();
        }

        public int Evaluate(int[] pits)
        {
            for (var i=0; i<INPUT_NEURONS; i++)
            {
                _inputNeurons[i].AcceptAwariPits(pits);
            }

            for (var i = 0; i < FIRST_LAYER_NEURONS; i++)
            {
                _firstLayerIntermediateNeurons[i].AcceptSignal(_inputNeurons);
            }

            for (var i = 0; i < SECOND_LAYER_NEURONS; i++)
            {
                _secondLayerIntermediateNeurons[i].AcceptSignal(_firstLayerIntermediateNeurons);
            }
            
            for (var i = 0; i < OUTPUT_NEURONS; i++)
            {
                _outputNeurons[i].AcceptSignal(_secondLayerIntermediateNeurons);
            }

            return _outputNeurons.OrderByDescending(x => x.Value).First().Index - 48;
        }

        public void Learn(int[] pits, int score)
        {
            throw new NotImplementedException();
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