using NeuralNetworksAwari.AwariEngine.NeuralNetworks.Interfaces;
using System.Linq;

namespace NeuralNetworksAwari.AwariEngine.NeuralNetworks
{
    public class Brain: IBrain
    {
        private const int NUMBER_OF_AWARI_PARAMETERS = 12 * 47 + 48;

        private const int NUMBER_OF_INPUT_NEURONS = 6 * 48;
        private const string INPUT_FILE = "input_";

        private const int NUMBER_OF_INTERMEDIATE_NEURONS = 4 * 48;
        private const string INTERMEDIATE_FILE = "intermediate_";

        private const int NUMBER_OF_OUTPUT_NEURONS = 2 * 48 + 1;
        private const string OUTPUT_FILE = "output_";

        private const double LEARN_FACTOR = 0.01d;

        private readonly IWeightingFactorsRepository _repository;
        private InputNeuron[] _inputs;
        private IntermediateNeuron[] _intermediates;
        private OutputNeuron[] _outputs;

        public Brain(IWeightingFactorsRepository repository)
        {
            _repository = repository;
        }

        public void BuildNeuronLayers()
        { 
            if (_repository.HasWeightingFactors)
            {
                _inputs = Enumerable.Range(0, NUMBER_OF_INPUT_NEURONS)
                    .ToList()
                    .Select(x => new InputNeuron(x, _repository.Read(INPUT_FILE, x)))
                    .ToArray();

                _intermediates = Enumerable
                    .Range(0, NUMBER_OF_INTERMEDIATE_NEURONS).ToList()
                    .Select(x => new IntermediateNeuron(x, _repository.Read(INTERMEDIATE_FILE, x),_inputs))
                    .ToArray();

                _outputs = Enumerable.Range(0, NUMBER_OF_OUTPUT_NEURONS)
                    .ToList()
                    .Select(x => new OutputNeuron(x, _repository.Read(OUTPUT_FILE, x), _intermediates))
                    .ToArray();
            }
            else
            {
                _inputs = Enumerable.Range(0, NUMBER_OF_INPUT_NEURONS)
                    .ToList()
                    .Select(x => new InputNeuron(x, _repository.Create(NUMBER_OF_AWARI_PARAMETERS)))
                    .ToArray();

                _intermediates = Enumerable.Range(0, NUMBER_OF_INTERMEDIATE_NEURONS)
                    .ToList()
                    .Select(x => new IntermediateNeuron(x, _repository.Create(NUMBER_OF_INPUT_NEURONS), _inputs))
                    .ToArray();

                _outputs = Enumerable.Range(0, NUMBER_OF_OUTPUT_NEURONS)
                    .ToList()
                    .Select(x => new OutputNeuron(x, _repository.Create(NUMBER_OF_INTERMEDIATE_NEURONS), _intermediates))
                    .ToArray();
            }
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
            var expectedIndex = score + 48;
            
            for (var i=0; i<97; i++)
            {
                if (scores[i].Index == expectedIndex)
                {
                    scores[i].Learn(LEARN_FACTOR);
                }
            }
        }
        
        public void StoreWeightFactors()
        {
            _repository.Store(INPUT_FILE, _inputs);
            _repository.Store(INTERMEDIATE_FILE, _intermediates);
            _repository.Store(OUTPUT_FILE, _outputs);
        }
    }
}