using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NeuralNetworksAwari.AwariEngine.NeuralNetwork;
using NeuralNetworksAwari.AwariEngine.NeuralNetwork.Interfaces;
using NSubstitute;
using System.Collections.Generic;
using System.Linq;

namespace NeuralNetworksAwari.AwariEngineTests.NeuralNetworks
{
    [TestClass]
    public class InputNeuronTests
    {
        private IRandomizer _random;

        [TestInitialize]
        public void Initialize()
        {
            _random = Substitute.For<IRandomizer>();
            _random.GetDouble().Returns(1);
        }

        [TestMethod]
        public void When_passing_an_Awari_position_the_input_neuron_calculates_a_value_equal_to_the_number_of_stones_when_all_weighting_factors_are_1()
        {
            var inputNeuron = new InputNeuron(new List<IOutputNeuron>(), _random);

            var awariPits = new [] { 4, 4, 4, 4, 4, 4, 8, 3, 0, 0, 4, 2 };

            inputNeuron.Accept(awariPits);

            inputNeuron.Value.Should().Be(awariPits.Sum());
        }

        [TestMethod]
        public void Input_neuron_can_pass_its_value_to_an_output_neuron_with_all_weighting_factors_set_to_1()
        {
            var random = Substitute.For<IRandomizer>();
            random.GetDouble().Returns(1);

            var neuron = new OutputNeuron(random);
            var inputNeuron = new InputNeuron(new List<IOutputNeuron> { neuron }, _random);

            var awariPits = new[] { 4, 4, 4, 4, 4, 4, 8, 3, 0, 0, 4, 2 };
            inputNeuron.Accept(awariPits);
            inputNeuron.SendSignal();

            neuron.Value.Should().Be(awariPits.Sum());
        }

        [TestMethod]
        public void Can_pass_a_value_to_multiple_layers()
        {
            var random = Substitute.For<IRandomizer>();
            random.GetDouble().Returns(1);

            var outputNeuron = new OutputNeuron(random);
            var neuron = new OutputNeuron(new List<IOutputNeuron> { outputNeuron }, _random);
            var inputNeuron = new InputNeuron(new List<IOutputNeuron> { neuron }, _random);

            var awariPits = new[] { 4, 4, 4, 4, 4, 4, 8, 3, 0, 0, 4, 2 };

            inputNeuron.Accept(awariPits);
            inputNeuron.SendSignal();
            neuron.SendSignal();

            outputNeuron.Value.Should().Be(awariPits.Sum());
        }
    }
}
