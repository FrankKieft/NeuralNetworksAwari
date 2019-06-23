using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NeuralNetworksAwari.AwariEngine.NeuralNetworks;
using NeuralNetworksAwari.AwariEngine.NeuralNetworks.Interfaces;
using NeuralNetworksAwari.AwariEngine.Util;
using NSubstitute;
using System;
using System.IO;

namespace NeuralNetworksAwari.AwariEngineTests.NeuralNetworks
{
    [TestClass]
    public class WeightingFactorRepositoryTests
    {
        private const double FACTOR = 0.298374d;

        private IWeightingFactorsRepository _repository;
        private INeuron _neuron;
        private INeuron[] _neurons;
        private IRandomizer _randomizer;

        [TestInitialize]
        public void Initialize()
        {
            _neuron = Substitute.For<INeuron>();
            _neuron.WeightingFactors.Returns(new[] { FACTOR });
            _neurons = new[] { _neuron };
            _randomizer = Substitute.For<IRandomizer>();
            _repository = new WeightingFactorsRepository(_randomizer);
        }

        [TestMethod]
        public void Can_store_new_weight_factors()
        {
            const string name = "testStore";
            var file = $"{Environment.CurrentDirectory}\\factors\\{name}0";

            _repository.DeleteAll();
            _repository.Store(name, _neurons);

            File.Exists(file).Should().BeTrue();
            File.ReadAllText(file).TrimEnd().Should().Be(FACTOR.ToString());
        }

        [TestMethod]
        public void Can_overwrite_weight_factors()
        {
            const string name = "testOverwrite";
            const double newFactor = 0.90384d;
            var file = $"{Environment.CurrentDirectory}\\factors\\{name}0";

            _repository.Store(name, _neurons);

            _neuron.WeightingFactors.Returns(new[] { newFactor });

            _repository.Store(name, _neurons);

            File.Exists(file).Should().BeTrue();
            File.ReadAllText(file).TrimEnd().Should().Be(newFactor.ToString());
        }

        [TestMethod]
        public void Can_read_weight_factors()
        {
            const string name = "testRead";
            _repository.Store(name, _neurons);

            var factors = _repository.Read(name, 0);

            factors.Length.Should().Be(1);
            factors[0].Should().Be(FACTOR);
        }
        
        [TestMethod]
        public void Can_create_weight_factors()
        {
            const double newFactor = 0.2200134d;

            _randomizer.GetDouble().Returns(newFactor);

            var factors = _repository.Create(5);

            factors.Length.Should().Be(5);
            factors[0].Should().Be(newFactor);
        }

        [TestMethod]
        public void Can_delete_all_stored_weight_factors()
        {
            const string name = "testDelete";
            var file = $"{Environment.CurrentDirectory}\\factors\\{name}0";

            _repository.Store(name, _neurons);
            File.Exists(file).Should().BeTrue();

            _repository.DeleteAll();

            File.Exists(file).Should().BeFalse();
        }

        [TestMethod]
        public void Can_check_if_factors_exist()
        {
            const string name = "testIfExists";
            
            _repository.Store(name, _neurons);
            _repository.HasWeightingFactors.Should().BeTrue();

            _repository.DeleteAll();
            _repository.HasWeightingFactors.Should().BeFalse();
        }
    }
}
