using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NeuralNetworksAwari.AwariEngine.NeuralNetworks;
using NeuralNetworksAwari.AwariEngine.NeuralNetworks.Interfaces;
using NSubstitute;

namespace NeuralNetworksAwari.AwariEngineTests.NeuralNetworks
{
    [TestClass]
    public class LearningOutputNeuronTests
    {
        [TestMethod]
        public void OutputNeuron_increases_the_weighting_factors_with_1_percent_multiplied_by_neuron_value()
        {
            // Arrange            
            var neuron = GetLearningNeuron(0.9d);

            var learningNeuron = new LearningOutputNeuron(
                0,
                new[] { 0.5d },
                new[] { neuron });

            // Act
            learningNeuron.Learn(0.01d);

            // Assert
            learningNeuron.WeightingFactors[0].Should().Be(0.5 + 0.9 * 0.01);
        }

        [TestMethod]
        public void OutputNeuron_decreases_the_weighting_factors_with_1_percent_multiplied_by_weighting_factor_if_no_signal()
        {
            // Arrange            
            var neuron = GetLearningNeuron(0d);

            var learningNeuron = new LearningOutputNeuron(
                0,
                new[] { 0.5d },
                new[] { neuron });

            // Act
            learningNeuron.Learn(0.01d);

            // Assert
            learningNeuron.WeightingFactors[0].Should().Be(0.5 - 0.5 * 0.01);
        }

        private ILearning GetLearningNeuron(double value)
        {
            var neuron = Substitute.For<ILearning>();
            neuron.Value.Returns(value);
            neuron.Index.Returns(0);
            return neuron;
        }
    }
}
