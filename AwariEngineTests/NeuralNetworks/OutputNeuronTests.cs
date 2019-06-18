using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NeuralNetworksAwari.AwariEngine.NeuralNetworks;
using NeuralNetworksAwari.AwariEngine.NeuralNetworks.Interfaces;
using NSubstitute;

namespace NeuralNetworksAwari.AwariEngineTests.NeuralNetworks
{
    [TestClass]
    public class OutputNeuronTests
    {
        private const double FACTOR = .55d;

        [TestMethod]
        public void When_passing_a_signal_neuron_calculates_the_correct_percentage_based_on_weight_factor()
        {
            NeuronTest(signal: true, expected: FACTOR);
        }

        [TestMethod]
        public void When_not_passing_a_signal_neuron_calculates_a_percentage_of_0()
        {
            NeuronTest(signal: false, expected: 0);
        }

        [TestMethod]
        public void OutputNeuron_increases_the_weighting_factors_with_1_percent_if_signal()
        {
            LearningTest(true, 0.51d);
        }

        [TestMethod]
        public void OutputNeuron_decreases_the_weighting_factors_with_1_percent_if_no_signal()
        {
            LearningTest(false, 0.49d);
        }

        private void LearningTest(bool signal, double expected)
        {
            var neuron = Substitute.For<ISender>();
            neuron.Signal.Returns(signal);
            neuron.Index.Returns(0);

            var outputNeuron = new OutputNeuron(
            0,
            new[] { 0.5d },
            new[] { neuron });

            outputNeuron.Learn(0.01d);

            outputNeuron.WeightingFactors[0].Should().Be(expected);
        }

        private void NeuronTest(bool signal, double expected)
        {
            // Arrange            
            var n1 = Substitute.For<ISender>();
            n1.Signal.Returns(signal);
            n1.Index.Returns(0);

            var n2 = Substitute.For<ISender>();
            n2.Signal.Returns(false);
            n2.Index.Returns(0);

            var outputNeuron = new OutputNeuron(0, new[] { FACTOR }, new[] { n1, n2 });

            // Act
            outputNeuron.AcceptSignal();

            // Assert
            outputNeuron.Value.Should().Be(expected);
        }
    }
}
