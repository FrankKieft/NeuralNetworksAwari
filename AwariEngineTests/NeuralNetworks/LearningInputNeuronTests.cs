using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NeuralNetworksAwari.AwariEngine.NeuralNetworks;
using System.Linq;

namespace NeuralNetworksAwari.AwariEngineTests.NeuralNetworks
{
    [TestClass]
    public class LearningInputNeuronTests
    {
        private double[] _weightingFactors;

        [TestInitialize]
        public void Initialize()
        {
            _weightingFactors = Enumerable.Range(0, 12 * 48).ToList().Select(x => 0.5d).ToArray();
        }

        [TestMethod]
        public void InputNeuron_decreases_the_weighting_factors_with_1_percent_when_input_signal_was_1()
        {
            // Arrange            
            var neuron = new LearningInputNeuron(
                0,
                _weightingFactors);

            // Act
            neuron.AcceptAwariPits(new[] { 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 });
            neuron.Learn(0.01d);

            // Assert
            neuron.WeightingFactors[0].Should().Be(0.505);
        }

        [TestMethod]
        public void InputNeuron_decreases_the_weighting_factors_with_1_percent_when_input_signal_was_0()
        {
            // Arrange            
            var neuron = new LearningInputNeuron(
                0,
                _weightingFactors);

            // Act
            neuron.AcceptAwariPits(new[] { 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 });
            neuron.Learn(0.01d);

            // Assert
            neuron.WeightingFactors[0].Should().Be(0.495);
        }
    }
}
