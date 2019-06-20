using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NeuralNetworksAwari.AwariEngine.NeuralNetworks.Interfaces;
using NSubstitute;
using System;
using System.IO;

namespace NeuralNetworksAwari.AwariEngineTests.NeuralNetworks
{
    [TestClass]
    public class FactorRepositoryTests
    {
        [TestInitialize]
        public void Initialize()
        {
        }

        [TestMethod]
        public void Can_store_weight_factors()
        {
            const string name = "test";
            var file = $"{Environment.CurrentDirectory}\\factors\\{name}0";

            var repository = new FactorsRepository();

            var neuron = Substitute.For<INeuron>();
            neuron.WeightingFactors.Returns(new[] { 0.2d });

            var neurons = new[] { neuron };

            repository.Store("test", new[] { neuron });

            File.Exists(file).Should().BeTrue();
            File.ReadAllText(file).Should().Be("0.2");
        }
    }
}
