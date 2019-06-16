using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NeuralNetworksAwari.AwariEngine.NeuralNetworks.Interfaces;
using NSubstitute;

namespace NeuralNetworksAwari.AwariEngineTests.NeuralNetworks
{
    [TestClass]
    public class BrainTests
    {
        private int[] _testPosition;
        private IRandomizer _randomizer;

        [TestInitialize]
        public void Initialize()
        {
            _randomizer = Substitute.For<IRandomizer>();
            _randomizer.GetDouble().Returns(0.5d);
            _testPosition =new int[] { 1,2,3,4,5,6,7,8,3,3,3,3};
        }

        [TestMethod]
        public void When_I_give_an_Awari_position_to_the_brain_I_get_a_possible_score_back()
        {
            var brain = new Brain(_randomizer);

            var score = brain.Evaluate(_testPosition);

            score.Should().Be(-48);
        }
    }
}
