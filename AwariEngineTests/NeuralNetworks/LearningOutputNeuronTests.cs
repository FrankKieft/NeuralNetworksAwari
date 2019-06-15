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
    public class LearningOutputNeuronTests
    {
        [TestMethod]
        public void OutputNeuron_increases_the_weighting_factors_with_1_percent_multiplied_by_neuron_value()
        {
            // Arrange
            
            var neuron1 = GetNeuron("0000000000000000000000000000000B", 0.9d);

            var neuron = new OutputNeuron(Guid.Empty, new Dictionary<Guid, double>{
                { neuron1.Key, 0.5d}
            });
                       
            var learningNeuron = new LearningOutputNeuron(neuron);

            // Act
            learningNeuron.AcceptSignal(new[] { neuron1,  });
            learningNeuron.Learn(0.01d);

            // Assert
            learningNeuron.WeightingFactors[neuron1.Key].Should().Be(0.5 + 0.9 * 0.01);
        }

        [TestMethod]
        public void OutputNeuron_decreases_the_weighting_factors_with_1_percent_multiplied_by_weighting_factor_if_no_signal()
        {
            // Arrange

            var neuron1 = GetNeuron("0000000000000000000000000000000B", 0d);
            
            var neuron = new OutputNeuron(Guid.Empty, new Dictionary<Guid, double>{
                { neuron1.Key, 0.5d}
            });

            var learningNeuron = new LearningOutputNeuron(neuron);

            // Act
            learningNeuron.AcceptSignal(new[] { neuron1 });
            learningNeuron.Learn(0.01d);

            // Assert
            learningNeuron.WeightingFactors[neuron1.Key].Should().Be(0.5 - 0.5 * 0.01);
        }

        private ILearningOutputNeuron GetNeuron(string key, double value)
        {
            var neuron = Substitute.For<ILearningOutputNeuron>();
            neuron.Value.Returns(value);
            neuron.Key.Returns(Guid.Parse(key));
            return neuron;
        }
    }
}
