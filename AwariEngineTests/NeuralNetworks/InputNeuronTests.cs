using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NeuralNetworksAwari.AwariEngine.NeuralNetworks;
using System.Linq;

namespace NeuralNetworksAwari.AwariEngineTests.NeuralNetworks
{
    [TestClass]
    public class InputNeuronTests
    {
        [TestMethod]
        public void When_passing_an_Awari_position_the_input_neuron_passes_a_signal_when_all_weighting_factors_are_above_half()
        {
            NeuronTest(.06d, false);
        }

        [TestMethod]
        public void When_passing_an_Awari_position_the_input_neuron_passes_a_signal_when_all_weighting_factors_are_below_half()
        {
            NeuronTest(.04d, false);
        }

        [TestMethod]
        public void InputNeuron_can_increases_the_weighting_factors_for_positives_with_1_percent()
        {
            LearningTest(0.01d);
        }

        [TestMethod]
        public void InputNeuron_can_decrease_the_weighting_factors_for_positives_with_1_percent()
        {
            LearningTest(-0.01d);
        }

        private void LearningTest(double factor)
        {
            var pits = new[] { 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
            var weightingFactors = Enumerable.Range(0, 12 * 47 + 48).ToList().Select(x => 0.5d).ToArray();
            var neuron = new InputNeuron(0, weightingFactors);
            neuron.AcceptAwariPits(pits, 47);
            neuron.Learn(factor);

            neuron.WeightingFactors[46].Should().Be(0.5d + factor);
            neuron.WeightingFactors[47].Should().Be(0.5d - factor);
            neuron.WeightingFactors[48].Should().Be(0.5d + factor);
            neuron.WeightingFactors[49].Should().Be(0.5d - factor);
        }

        private void NeuronTest(double factor, bool expected)
        {
            // Arrange
            var weightingFactors = Enumerable.Range(0, 12 * 47 + 48).ToList().Select(x => factor).ToArray();
            var awariPits = new[] { 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1 };

            var inputNeuron = new InputNeuron(0, weightingFactors);

            // Act
            inputNeuron.AcceptAwariPits(awariPits, 36);

            // Assert
            inputNeuron.Signal.Should().Be(expected);
        }
    }
}
