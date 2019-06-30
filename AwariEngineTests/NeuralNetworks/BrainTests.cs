using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NeuralNetworksAwari.AwariEngine.NeuralNetworks;
using NeuralNetworksAwari.AwariEngine.NeuralNetworks.Interfaces;
using NeuralNetworksAwari.AwariEngine.Util;
using NSubstitute;
using System;
using System.Linq;

namespace NeuralNetworksAwari.AwariEngineTests.NeuralNetworks
{
    [TestClass]
    public class BrainTests
    {
        private int[] _testPosition;
        private IRandomizer _randomizer;
        private IWeightingFactorsRepository _repository;
        private IBrain _brain;

        [TestInitialize]
        public void Initialize()
        {
            _randomizer = Substitute.For<IRandomizer>();
            _repository = new WeightingFactorsRepository(_randomizer);
            _repository.DeleteAll();
            _testPosition = new int[] { 1, 2, 3, 4, 5, 6, 7, 8, 3, 3, 3, 3 };
            _brain = new Brain(_repository);
        }

        [TestMethod]
        public void When_I_give_an_Awari_position_to_the_brain_I_get_a_possible_score_back()
        {
            BrainTest(0.5d, 1d);
        }

        [TestMethod]
        public void If_all_weighting_values_are_set_below_the_threshold_of_0_5_the_score_is_0()
        {
            BrainTest(0.4d, 0d);
        }

        [TestMethod]
        public void The_brain_can_learn_because_likelyness_of_preferred_outcome_is_higher()
        {
            var brain = new Brain(new WeightingFactorsRepository(new Randomizer()), learningFactor: 0.01d);
            brain.BuildNeuronLayers();
            
            var scores = brain.Evaluate(_testPosition, 0);
            var before = scores[48].Value;
            WriteScores(scores);

            brain.Learn(_testPosition, 0, 0);

            scores = brain.Evaluate(_testPosition, 0);
            var after = scores[48].Value;
            WriteScores(scores);

            after.Should().BeGreaterThan(before);
        }

        private void WriteScores(IOutput[] scores)
        {
            var orderedScores = scores.ToList().OrderByDescending(x => x.Value).ToArray();
            
            Console.WriteLine("Scores:");
            Console.WriteLine("=======");
            for (var i = 0; i < scores.Length; i++)
            {
                Console.WriteLine($"{GetIndex(scores, i)} - {GetScore(scores, i)}          {GetIndex(orderedScores, i)} - {GetScore(orderedScores, i)}");
            }
            Console.WriteLine("");
        }

        private string GetIndex(IOutput[] s, int i) => $"    {s[i].Index}".Substring($"{s[i].Index}".Length);
        private string GetScore(IOutput[] s, int i) => $"{Math.Round(s[i].Value, 5)}0000".Substring(0,7);

        [TestMethod]
        public void Can_store_weightfactors()
        {
            _brain.BuildNeuronLayers();

            _repository.HasWeightingFactors.Should().BeFalse();

            _brain.StoreWeightFactors();

            _repository.HasWeightingFactors.Should().BeTrue();
        }

        private void BrainTest(double factor, double expected)
        {
            _randomizer.GetDouble().Returns(factor);
            _brain.BuildNeuronLayers();

            var scores = _brain.Evaluate(_testPosition, 0);

            scores[48].Value.Should().Be(expected);
        }
    }
}
