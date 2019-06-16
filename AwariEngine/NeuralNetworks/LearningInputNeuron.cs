using NeuralNetworksAwari.AwariEngine.NeuralNetworks.Interfaces;

namespace NeuralNetworksAwari.AwariEngine.NeuralNetworks
{
    public class LearningInputNeuron : InputNeuron, ILearning
    {
        private int[] _pits;

        public LearningInputNeuron(int index, double[] weightingFactors):base(index,weightingFactors)
        {
        }

        public override void AcceptAwariPits(int[] pits)
        {
            _pits = pits;
            base.AcceptAwariPits(pits);
        }

        public void Learn(double factor)
        {
            for (var i = 0; i < 572; i++)
            {
                var pit = i / 48;
                var stones = i % 48;
                if (_pits[pit] > stones)
                {
                    WeightingFactors[i] *= 1+factor;
                }
                else
                {
                    WeightingFactors[i] *= 1-factor;
                }
            }
        }
    }
}
