using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NeuralNetworksAwari.AwariEngine;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NeuralNetworksAwari.AwariEngineTests
{
    [TestClass]
    public partial class AwariParallelEvaluationTests
    {
        private const int DEPTH = 8;

        [TestMethod]
        public void I_can_get_the_best_score_for_the_initial_board_10_half_moves_deep_in_parallel()
        {
            var boards = new List<AwariBoard>();
            var eval = new List<TestResult>();

            Enumerable.Range(0, 36).ToList().ForEach(x => boards.Add(AwariBoard.GetInitialBoard()));
            Parallel.For(0, 36, i =>
            {
                var firstMove = ((char)('A' + (i / 6))).ToString();
                var secondMove = ((char)('a' + (i % 6))).ToString();

                if (boards[i].CanSow(firstMove))
                {
                    boards[i].Sow(firstMove);
                    if (boards[i].CanSow(secondMove))
                    {
                        eval.Add(new TestResult
                        {
                            Index = i,
                            FirstMove = firstMove,
                            SecondMove = secondMove,
                            Evaluation = boards[i].Sow(secondMove).Evaluate(DEPTH)
                        });
                    }
                    else
                    {
                        eval.Add(new TestResult
                        {
                            Index = i,
                            FirstMove = firstMove,
                            Evaluation = boards[i].NorthAwari - boards[i].SouthAwari
                        });
                    }
                }
            });

            eval.GroupBy(x => x.FirstMove)
                .Select(x => new TestResult { FirstMove = x.Key, Evaluation = x.Min(y => y.Evaluation) })
                .OrderByDescending(x => x.Evaluation)
                .First()
                .Evaluation.Should().Be(-1);
        }
    }
}
