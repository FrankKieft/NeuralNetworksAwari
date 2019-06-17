using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NeuralNetworksAwari.AwariEngine;
using NeuralNetworksAwari.AwariEngine.Evaluations;

namespace NeuralNetworksAwari.AwariEngineTests.Evaluations
{
    [TestClass]
    public class EvaluationTests
    {
        private AwariEvaluator _evaluator;

        [TestInitialize]
        public void Initialize()
        {
            _evaluator = new AwariEvaluator();
        }

        [TestMethod]
        public void Returns_the_best_score_in_case_of_a_grand_slam()
        {
            var board = new AwariBoard(
                F: 2,
                a: 1,
                b: 2,
                southAwari: 24,
                northAwari: 19);

            board.Evaluate(_evaluator, 12).Should().Be(10);
        }

        [TestMethod]
        public void Returns_the_best_score_when_game_ended_after_repetition_of_position()
        {
            var board = new AwariBoard(
                F: 1,
                f: 1,
                southAwari: 22,
                northAwari: 24);

            board.Evaluate(_evaluator, 12).Should().Be(-2);
        }

        [TestMethod]
        public void Returns_the_best_score_for_6_stones()
        {
            var board = new AwariBoard(
                A: 1, E: 1, F: 2,
                a: 2,
                southAwari: 21,
                northAwari: 21);

            board.Evaluate(_evaluator, 12).Should().Be(4);
        }

        /// <summary>
        /// Creates a pit with 13 on F, but after the capture of 4 
        /// North can immediatly capture 2 back.
        /// </summary>
        [TestMethod]
        public void Returns_the_best_score_for_14_stones_after_4_moves()
        {
            var board = new AwariBoard(
                D: 1, E: 1, F: 11,
                a: 1,
                southAwari: 17,
                northAwari: 17);

            board.Evaluate(_evaluator, 8).Should().Be(2);
        }

        [TestMethod]
        public void Illegal_move_is_recognized_by_the_evaluator_and_not_played()
        {
            // F cannot be played, no gain for South
            var board = new AwariBoard(
                D: 2, F: 4,
                a: 2, b: 2, c: 2, d: 2,
                southAwari: 17,
                northAwari: 17);

            board.Evaluate(_evaluator, 4).Should().Be(0);
        }

        [TestMethod]
        public void Leaving_North_with_no_legal_move_is_allowed_and_move_played()
        {
            // F can be played, gain of 12
            var board = new AwariBoard(
                F: 4,
                a: 2, b: 2, c: 2, d: 2,
                southAwari: 18,
                northAwari: 18);

            board.Evaluate(_evaluator, 4).Should().Be(12);
        }

        [TestMethod]
        public void I_can_get_the_best_score_for_the_initial_board_4_moves()
        {
            var board = AwariBoard.GetInitialBoard();

            board.Evaluate(_evaluator, 8).Should().Be(0);
        }
    }
}
