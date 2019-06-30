using NeuralNetworksAwari.AwariEngine.NeuralNetworks.Interfaces;

namespace NeuralNetworksAwari.AwariEngine.NeuralNetworks
{
    public class OutputNeuron : IOutput
    {
        private ISender[] _previousNeuronLayer;
        private double _denominator;

        public OutputNeuron(int index, double[] weightingFactors, ISender[] previousNeuronLayer)
        {
            _previousNeuronLayer = previousNeuronLayer;
            _denominator = _previousNeuronLayer.Length / 2d;
            Index =index;
            WeightingFactors = weightingFactors;
        }
        
        public double Value { get; protected set; }
        public int Index { get; }
        public double[] WeightingFactors { get; }

        public void AcceptSignal()
        {
            Value = 0;
            for (var i = 0; i < _previousNeuronLayer.Length; i++)
            {
                if (_previousNeuronLayer[i].Signal)
                {
                    Value += WeightingFactors[i];
                }
            }
            Value /= _denominator;
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
