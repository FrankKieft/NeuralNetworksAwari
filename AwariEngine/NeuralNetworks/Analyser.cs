using NeuralNetworksAwari.AwariEngine.NeuralNetworks.Interfaces;
using System;
using System.Diagnostics;
using System.IO;
using System.Linq;

namespace NeuralNetworksAwari.AwariEngine.NeuralNetworks
{
    public class Analyser: IDisposable
    {
        private IBrain _brain;
        private Stopwatch _stopwatch;
        private StreamWriter _writer;
        private IOutput[] _scores;
        private int _index;
        private int[] _pits;
        private int _capturedStones;

        public Analyser(IBrain brain)
        {
            _brain = brain;
            _stopwatch = new Stopwatch();

            var directory = $"{Environment.CurrentDirectory}\\analyses";
            if (!Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }
            var file = $"{directory}\\analyses{DateTime.Now.ToString("yyyyMMddHHmmss")}";
            if (File.Exists(file))
            {
                File.Delete(file);
            }
            _writer = new StreamWriter(file);
            _writer.WriteLine($"index;captured;milliseconds;score;value;place;top5-1;top5-2;top5-3;top5-4;top5-5;A;B;C;D;E;F;a;b;c;d;e;f");
        }

        public void Dispose()
        {
            _writer.Close();
            _writer.Dispose();
            _writer = null;
        }

        public void Start(int index, int[] pits, int capturedStones)
        {
            _index = index;
            _pits = pits;
            _capturedStones = capturedStones;
            _scores = _brain.Evaluate(pits, capturedStones);
            _stopwatch.Restart();
        }

        public void Stop(int score)
        {
            _stopwatch.Stop();
            var ordered = _scores.ToList().OrderByDescending(x => x.Value).ToArray();
            var place = 0;
            for (var i = 0; i < ordered.Length ;i++)
            {
                if (ordered[i].Index == score + 48)
                    place = i;
            }
            _writer.WriteLine($"{_index};{48 - _capturedStones};{_stopwatch.ElapsedMilliseconds};{score};{_scores[score + 48].Value.ToString()};{place};{ordered[0].Index};{ordered[1].Index};{ordered[2].Index};{ordered[3].Index};{ordered[4].Index};{string.Join(';',_pits)}");
            _writer.Flush();
        }
    }
}
