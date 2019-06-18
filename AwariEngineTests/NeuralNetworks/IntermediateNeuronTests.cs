using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NeuralNetworksAwari.AwariEngine.NeuralNetworks;
using NeuralNetworksAwari.AwariEngine.NeuralNetworks.Interfaces;
using NSubstitute;

namespace NeuralNetworksAwari.AwariEngineTests.NeuralNetworks
{
    [TestClass]
    public class IntermediateNeuronTests
    {
        private const double FACTOR = .55d;

        [TestMethod]
        public void When_passing_a_signal_neuron_passes_a_signal_if_weighting_factor_is_above_0_25()
        {
            NeuronTest(signal: true, factor: 0.3d, expected: true);
        }

        [TestMethod]
        public void When_passing_a_signal_neuron_passes_no_signal_if_weighting_factor_is_below_0_25()
        {
            NeuronTest(signal: true, factor: 0.2d, expected: false);
        }

        [TestMethod]
        public void When_not_passing_a_signal_neuron_passes_no_signal()
        {
            NeuronTest(signal: false, factor: 0.9d, expected: false);
        }

        [TestMethod]
        public void Neuron_increases_the_weighting_factors_with_1_percent_if_signal()
        {
            LearningTest(true, 0.51d);
        }

        [TestMethod]
        public void Neuron_decreases_the_weighting_factors_with_1_percent_if_no_signal()
        {
            LearningTest(false, 0.49d);
        }

        private void LearningTest(bool signal, double expected)
        {
            var neuron = Substitute.For<ISender>();
            neuron.Signal.Returns(signal);
            neuron.Index.Returns(0);

            var intermediate = new IntermediateNeuron(
            0,
            new[] { 0.5d },
            new[] { neuron });

            intermediate.Learn(0.01d);

            intermediate.WeightingFactors[0].Should().Be(expected);
        }

        private void NeuronTest(bool signal, double factor, bool expected)
        {
            // Arrange            
            var neuron = Substitute.For<ISender>();
            neuron.Signal.Returns(signal);
            neuron.Index.Returns(0);

            var intermediateNeuron = new IntermediateNeuron(0, new[] { factor }, new[] { neuron });

            // Act
            intermediateNeuron.AcceptSignal();

            // Assert
            intermediateNeuron.Signal.Should().Be(expected);
        }
    }
}
