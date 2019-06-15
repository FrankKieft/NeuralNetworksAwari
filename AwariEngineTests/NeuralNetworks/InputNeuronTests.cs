using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NeuralNetworksAwari.AwariEngine.NeuralNetworks;
using System;
using System.Linq;

namespace NeuralNetworksAwari.AwariEngineTests.NeuralNetworks
{
    [TestClass]
    public class InputNeuronTests
    {
        private double[] _weightingFactors;

        [TestInitialize]
        public void Initialize()
        {
            _weightingFactors = Enumerable.Range(0, 12 * 48).ToList().Select(x => 0.5d).ToArray();
        }

        [TestMethod]
        public void When_passing_an_Awari_position_the_input_neuron_calculates_a_value_that_is_equal_to_the_avarage_of_the_used_weighting_factors()
        {
            // Arrange
            var awariPits = new [] { 4, 4, 4, 4, 4, 4, 8, 8, 0, 0, 4, 4 };
            
            var inputNeuron = new InputNeuron(Guid.Empty, _weightingFactors);

            // Act
            inputNeuron.AcceptAwariPits(awariPits);

            // Assert
            inputNeuron.Value.Should().Be(0.5d);
        }
    }
}
