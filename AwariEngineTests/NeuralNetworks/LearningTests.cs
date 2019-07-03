using Microsoft.VisualStudio.TestTools.UnitTesting;
using NeuralNetworksAwari.AwariEngine.Evaluations;
using NeuralNetworksAwari.AwariEngine.NeuralNetworks;
using NeuralNetworksAwari.AwariEngine.Util;

namespace NeuralNetworksAwari.AwariEngineTests.NeuralNetworks
{
    [TestClass]
    public class LearningTests
    {
        [TestMethod]
        public void The_neural_network_improves_its_performance_by_learning()
        {
            var randomizer = new Randomizer();
            var teacher = new Teacher(randomizer, new AwariEvaluator());

            var brain = new Brain(new WeightingFactorsRepository(randomizer));
            brain.BuildNeuronLayers();

            teacher.Teach(brain, 1000);
        }
    }
}
