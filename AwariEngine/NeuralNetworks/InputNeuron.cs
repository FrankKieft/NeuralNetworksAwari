using NeuralNetworksAwari.AwariEngine.NeuralNetworks.Interfaces;

namespace NeuralNetworksAwari.AwariEngine.NeuralNetworks
{
    public class InputNeuron : IInput
    {
        private int _stonesCaptured;
        private int[] _pits;

        public InputNeuron(int index, double[] weightingFactors)
        {
            Index = index;
            WeightingFactors = weightingFactors;
        }

        public int Index { get; }
        public bool Signal { get; private set; }
        public double[] WeightingFactors { get; }

        public void AcceptAwariPits(int[] position, int stonesCaptured)
        {
            _stonesCaptured = stonesCaptured;
            _pits = position;
            var value = 0d;

            for (var i = 0; i < stonesCaptured; i++)
            {
                value += WeightingFactors[i];
                if (value >= 24d)
                {
                    Signal = true;
                    return;
                }
            }

            for (var i = 0; i < 564; i++)
            {
                var pit = i / 47;
                var stones = i % 47;
                if (position[pit] > stones)
                {
                    value += WeightingFactors[i + 48];
                    if (value >= 24d)
                    {
                        Signal = true;
                        return;
                    }
                }
            }

            Signal = false;
        }

        public void Learn(double factor)
        {
            for (var i = 0; i < 48; i++)
            {
                if (i < _stonesCaptured)
                {
                    WeightingFactors[i] += factor;
                }
                else
                {
                    WeightingFactors[i] -= factor;
                }
            }

            for (var i = 0; i < 564; i++)
            {
                var pit = i / 47;
                var stones = i % 47;
                if (_pits[pit] > stones)
                {
                    WeightingFactors[i + 48] += factor;
                }
                else
                {
                    WeightingFactors[i + 48] -= factor;
                }
            }
        }
    }
}
