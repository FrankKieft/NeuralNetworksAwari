using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using NeuralNetworksAwari.AwariEngine.NeuralNetworks.Interfaces;
using NeuralNetworksAwari.AwariEngine.Util;

namespace NeuralNetworksAwari.AwariEngine.NeuralNetworks
{
    public class WeightingFactorsRepository : IWeightingFactorsRepository
    {
        private string _directory;
        private IRandomizer _randomizer;

        public bool HasWeightingFactors
        {
            get
            {
                return Directory.Exists(_directory) && Directory.GetFiles(_directory).Length > 0;
            }
        }

        public WeightingFactorsRepository(IRandomizer randomizer)
        {
            _directory = $"{Environment.CurrentDirectory}\\factors";
            _randomizer = randomizer;
        }

        public void Store(string name, INeuron[] neurons)
        {
            if (!Directory.Exists(_directory))
            {
                Directory.CreateDirectory(_directory);
            }

            for (var i = 0; i < neurons.Length; i++)
            {
                using (var sw = new StreamWriter($"{_directory}\\{name}{i}"))
                {
                    foreach (var f in neurons[i].WeightingFactors)
                        sw.WriteLine($"{f}");
                }
            }
        }

        public double[] Read(string name, int i)
        {
            var file = $"{_directory}\\{name}{i}";
            var factors = new List<double>();

            if (!File.Exists(file))
            {
                return new double[0];
            }
            else
            {
                using (var reader = new StreamReader(file))
                {
                    while (!reader.EndOfStream)
                    {
                        var line = reader.ReadLine();
                        factors.Add(Convert.ToDouble(line));
                    }
                }
            }
            return factors.ToArray();
        }

        public double[] Create(int n)
        {
            var factors = new List<double>();

            for (var j = 0; j < n; j++)
            {
                factors.Add(_randomizer.GetDouble());
            }
            return factors.ToArray();
        }

        public void DeleteAll()
        {
            if (Directory.Exists(_directory))
            {
                var files = Directory.GetFiles(_directory);
                files.ToList().ForEach(x => File.Delete(x));
                Directory.Delete(_directory);
            }
        }
    }
}