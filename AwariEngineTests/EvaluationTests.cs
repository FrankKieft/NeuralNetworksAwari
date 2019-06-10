using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NeuralNetworksAwari.AwariEngine;

namespace NeuralNetworksAwari.AwariEngineTests
{
    [TestClass]
    public class EvaluationTests
    {
        [TestMethod]
        public void I_can_get_the_best_score_for_1_stone_after_1_moves()
        {
            var board = new AwariBoard(
                F: 2,
                a: 1,
                b: 2,
                southAwari: 24,
                northAwari: 19);

            board.Evaluate(1).Should().Be(10);
        }

        [TestMethod]
        public void I_can_get_the_best_score_for_2_stones_after_6_moves()
        {
            var board = new AwariBoard(
                F: 1, 
                f: 1,
                southAwari: 22, 
                northAwari: 24);

            board.Evaluate(12).Should().Be(-2);
        }

        [TestMethod]
        public void I_can_get_the_best_score_for_6_stones_after_5_moves()
        {
            var board = new AwariBoard(
                A: 1, E: 1, F: 2,
                a: 2,
                southAwari: 21,
                northAwari: 21);

            board.Evaluate(12).Should().Be(4);
        }

        /// <summary>
        /// Creates a pit with 13 on F, but after the capture of 4 
        /// North can immediatly capture 2 back.
        /// </summary>
        [TestMethod]
        public void I_can_get_the_best_score_for_14_stones_after_4_moves()
        {
            var board = new AwariBoard(
                D: 1, E: 1, F: 11,
                a: 1,
                southAwari: 17,
                northAwari: 17);

            board.Evaluate(8).Should().Be(2);
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

            board.Evaluate(4).Should().Be(0);
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

            board.Evaluate(4).Should().Be(12);
        }

        [TestMethod]
        public void I_can_get_the_best_score_for_the_initial_board_8_half_moves_deep()
        {
            var board = AwariBoard.GetInitialBoard();

            board.Evaluate(8).Should().Be(0);
        }
    }
}
