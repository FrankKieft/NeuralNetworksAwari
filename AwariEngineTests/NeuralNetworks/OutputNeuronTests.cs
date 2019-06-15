using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NeuralNetworksAwari.AwariEngine.NeuralNetworks;
using NeuralNetworksAwari.AwariEngine.NeuralNetworks.Interfaces;
using NSubstitute;
using System;
using System.Collections.Generic;

namespace NeuralNetworksAwari.AwariEngineTests.NeuralNetworks
{
    [TestClass]
    public class OutputNeuronTests
    {
        [TestMethod]
        public void When_passing_the_value_of_an_input_neuron_the_output_neuron_calculates_the_correct_value()
        {
            const double factor = .5d;
            const double inputValue = .7d;

            // Arrange
            var expected = factor * inputValue;

            var inputNeuron = Substitute.For<IInputNeuron>();
            inputNeuron.Value.Returns(inputValue);
            inputNeuron.Key.Returns(Guid.Parse("123456789012345678901234567890AB"));

            var outputNeuron = new OutputNeuron(Guid.Empty, new Dictionary<Guid, double> { { inputNeuron.Key, factor } });
            
            // Act
            outputNeuron.AcceptSignal(new [] { inputNeuron });

            // Assert
            outputNeuron.Value.Should().Be(expected);
        }

        [TestMethod]
        public void When_passing_the_value_of_an_output_neuron_the_output_neuron_calculates_the_correct_value()
        {
            const double factor = .5d;
            const double value = .7d;

            // Arrange
            var expected = factor * value;

            var neuron = Substitute.For<IOutputNeuron>();
            neuron.Value.Returns(value);
            neuron.Key.Returns(Guid.Parse("123456789012345678901234567890AB"));

            var outputNeuron = new OutputNeuron(Guid.Empty, new Dictionary<Guid, double> { { neuron.Key, factor } });

            // Act
            outputNeuron.AcceptSignal(new[] { neuron });

            // Assert
            outputNeuron.Value.Should().Be(expected);
        }
    }
}
