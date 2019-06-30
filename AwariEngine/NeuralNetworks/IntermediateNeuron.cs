using NeuralNetworksAwari.AwariEngine.NeuralNetworks.Interfaces;

namespace NeuralNetworksAwari.AwariEngine.NeuralNetworks
{
    public class IntermediateNeuron : IReceiver, ISender
    {
        private ISender[] _previousNeuronLayer;
        private double _threshold;

        public IntermediateNeuron(int index, double[] weightingFactors, ISender[] previousNeuronLayer)
        {
            _previousNeuronLayer = previousNeuronLayer;
            _threshold = _previousNeuronLayer.Length / 4d;
            Index = index;
            WeightingFactors = weightingFactors;
        }

        public bool Signal { get; protected set; }
        public int Index { get; }
        public double[] WeightingFactors { get; }

        public void AcceptSignal()
        {
            var value = 0d;
            for (var i = 0; i < _previousNeuronLayer.Length; i++)
            {
                if (_previousNeuronLayer[i].Signal)
                {
                    value += WeightingFactors[i];
                    if (value>=_threshold)
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
            foreach (var n in _previousNeuronLayer)
            {
                WeightingFactors[n.Index] += n.Signal ? factor : -factor;
            }
        }
    }
}
