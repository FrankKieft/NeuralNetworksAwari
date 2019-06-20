using System;
using System.IO;
using NeuralNetworksAwari.AwariEngine.NeuralNetworks.Interfaces;

namespace NeuralNetworksAwari.AwariEngineTests.NeuralNetworks
{
    public class FactorsRepository : IFactorRepository
    {
        public FactorsRepository()
        {
        }

        public void Store(string name, INeuron[] neurons)
        {
            var directory = $"{Environment.CurrentDirectory}\\factors";

            if (!Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }

            for (var i = 0; i < neurons.Length; i++)
            {
                using (var sw = new StreamWriter($"{directory}\\{name}{i}"))
                {
                    foreach (var f in neurons[i].WeightingFactors)
                        sw.WriteLine($"{f}");
                }
            }
        }
    }
}