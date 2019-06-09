using NeuralNetworksAwari.AwariEngine;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace NeuralNetworksAwari.AwariEngineTests
{
    /// <summary>
    /// All rules based on http://web.cecs.pdx.edu/~bart/cs510games-summer2000/hw4/bart-awari-rules.html
    /// </summary>
    [TestClass]
    public class AwariBoardMoveTests
    {
        private AwariBoard _initialBoard;

        [TestInitialize]
        public void Initialize()
        {
            _initialBoard = AwariBoard.GetInitialBoard();
        }

        [TestMethod]
        public void R08_the_players_alternate_moves()
        {
            _initialBoard.Sow("B");
            _initialBoard.FirstToMove.Should().Be(Player.North);
            _initialBoard.Invoking(x => x.Sow("F")).Should().Throw<ArgumentException>().WithMessage("South already moved, a player cannot make two moves in a row.");
        }

        [TestMethod]
        public void R10_the_south_player_moves_first()
        {
            _initialBoard.FirstToMove.Should().Be(Player.South);
            _initialBoard.Invoking(x => x.Sow("c")).Should().Throw<ArgumentException>().WithMessage("Only south can make the first move (A-F).");
        }

        [TestMethod]
        public void R11_the_players_alternate_in_sowing_the_stones_in_a_pit_of_their_choice_on_their_side_of_the_board_North_owns_pits_a_f_and_south_pits_A_F()
        {
            _initialBoard.CanSow("B").Should().BeTrue();
            _initialBoard.CanSow("b").Should().BeFalse();
            _initialBoard.Sow("C");
            _initialBoard.CanSow("B").Should().BeFalse();
            _initialBoard.CanSow("b").Should().BeTrue();
            _initialBoard.Invoking(x => x.Sow("B")).Should().Throw<ArgumentException>().WithMessage("South already moved, a player cannot make two moves in a row.");
        }

        [TestMethod]
        public void R12_a_pit_may_be_sown_if_it_contains_one_or_more_stones()
        {
            var board = new AwariBoard(
                f: 3, e: 3, d: 3, c: 3, b: 3, a: 3,
                A: 3, B: 0, C: 3, D: 3, E: 3, F: 3,
                southAwari: 6,
                northAwari: 9);
            
            board.CanSow("A").Should().BeTrue();
            board.CanSow("B").Should().BeFalse();
            board.CanSow("C").Should().BeTrue();
        }

        [TestMethod]
        public void R13_the_stones_are_removed_from_the_pit_and_placed_one_at_a_time_into_subsequent_pits_moving_around_the_board_counterclockwise_()
        {
            _initialBoard.Sow("D");
            _initialBoard.Pits["D"].Should().Be(0);
            _initialBoard.Pits["E"].Should().Be(5);
            _initialBoard.Pits["F"].Should().Be(5);
            _initialBoard.Pits["a"].Should().Be(5);
            _initialBoard.Pits["b"].Should().Be(5);
            _initialBoard.Pits["c"].Should().Be(4);
            _initialBoard.Sow("c");
            _initialBoard.Pits["c"].Should().Be(0);
            _initialBoard.Pits["e"].Should().Be(5);
        }

        [TestMethod]
        public void R14_the_original_pit_is_skipped_whenever_it_is_encountered()
        {
            var board = new AwariBoard(
                f: 3, e: 3, d: 3, c: 3, b: 3, a: 3,
                A: 3, B: 0, C: 3, D: 3, E: 15, F: 3,
                southAwari: 1,
                northAwari: 2);
            board.Sow("E");
            board.Pits["E"].Should().Be(0);
        }

        [TestMethod]
        public void R15_when_the_last_pebble_placed_makes_a_group_of_two_or_three_then_that_pits_stones_are_captured_and_scored_by_placing_in_the_capturing_players_awari()
        {
            CaptureTest(stonesInEndPit: 0, expectedStonesLeftInEndPit: 1, expectedStonesInSouthAwari: 0);
            CaptureTest(stonesInEndPit: 1, expectedStonesLeftInEndPit: 0, expectedStonesInSouthAwari: 2);
            CaptureTest(stonesInEndPit: 2, expectedStonesLeftInEndPit: 0, expectedStonesInSouthAwari: 3);
            CaptureTest(stonesInEndPit: 3, expectedStonesLeftInEndPit: 4, expectedStonesInSouthAwari: 0);
            CaptureTest(stonesInEndPit: 4, expectedStonesLeftInEndPit: 5, expectedStonesInSouthAwari: 0);
        }

        [TestMethod]
        public void R16_if_the_previous_pit_then_contains_a_group_of_two_or_three_stones_these_stones_are_also_captured_and_so_forth()
        {
            var board = new AwariBoard(
                f: 4, e: 4, d: 4, c: 1, b: 2, a: 1,
                A: 11, B: 4, C: 6, D: 4, E: 4, F: 3,
                southAwari: 0,
                northAwari: 0);

            board.Sow("C");

            board.Pits["a"].Should().Be(0);
            board.Pits["b"].Should().Be(0);
            board.Pits["c"].Should().Be(0);
            board.SouthAwari.Should().Be(7);
        }

        [TestMethod]
        public void R17_the_set_of_pits_which_are_captured_is_on_the_opponents_side_of_the_board_reachable_by_captures_from_the_last_pit_sown()
        {
            // Arrange
            var board = new AwariBoard(
                f: 4, e: 4, d: 4, c: 1, b: 2, a: 1,
                A: 13, B: 4, C: 6, D: 4, E: 4, F: 1,
                southAwari: 0,
                northAwari: 0);

            // Act
            board.Sow("C");

            // Assert
            board.Pits["F"].Should().Be(2);
            board.Pits["a"].Should().Be(0);
            board.Pits["b"].Should().Be(0);
            board.Pits["c"].Should().Be(0);
            board.SouthAwari.Should().Be(7);
        }

        /// <summary>
        /// The sowing of stones to capture all stones on the opponent's side of the board is known as a grand slam, 
        /// clean sweep, or grand coup. Normally, a grand slam ends the game, 
        /// capturing all stones remaining on the board.
        /// </summary>
        [TestMethod]
        public void R18_Stones_may_not_be_sown_for_a_grand_slam_unless_no_other_move_is_possible()
        {
            var board = new AwariBoard(
                f: 0, e: 0, d: 2, c: 1, b: 2, a: 1,
                A: 2, B: 0, C: 0, D: 0, E: 0, F: 4,
                southAwari: 18,
                northAwari: 18);

            board.CanSow("A").Should().BeTrue();
            board.CanSow("F").Should().BeFalse();

            board = new AwariBoard(
                d: 2, c: 1, b: 2, a: 1,
                F: 4,
                southAwari: 20,
                northAwari: 18);

            board.CanSow("F").Should().BeTrue();
        }

        [TestMethod]
        public void R19_a_game_will_be_ended_by_a_player_being_unable_to_move_in_which_case_the_remaining_stones_on_the_board_belong_to_the_opponent()
        {
            var board = new AwariBoard(
                A: 2, B: 1, C: 1,
                southAwari: 21,
                northAwari: 23);
            board.Sow("A");
            board.Pits["A"].Should().Be(0);
            board.Pits["B"].Should().Be(0);
            board.Pits["C"].Should().Be(0);
            board.GameHasEnded.Should().BeTrue();
            board.SouthAwari.Should().Be(25);
        }

        [TestMethod]
        public void R20_a_player_must_leave_the_opponent_with_a_legal_move_at_the_start_of_their_turn_if_it_is_possible_to_do_so()
        {
            var board = new AwariBoard(
                B: 4, F: 1,
                southAwari: 21,
                northAwari: 22);
            board.CanSow("B").Should().BeFalse();
            board.CanSow("F").Should().BeTrue();
        }

        [TestMethod]
        public void R21_a_game_will_also_be_ended_by_repetition_each_player_captures_the_stones_on_their_side_of_the_board()
        {
            var board = new AwariBoard(
                f: 1,
                F: 1,
                southAwari: 22,
                northAwari: 24);

            board.Sow("F").Sow("f").Sow("A").Sow("a").Sow("B").Sow("b").Sow("C").Sow("c").Sow("D").Sow("d").Sow("E");
            board.GameHasEnded.Should().BeFalse();
            board.SouthAwari.Should().Be(22);
            board.NorthAwari.Should().Be(24);

            board.Sow("e");
            board.GameHasEnded.Should().BeTrue();
            board.SouthAwari.Should().Be(23);
            board.NorthAwari.Should().Be(25);
        }

        private void CaptureTest(int stonesInEndPit, int expectedStonesLeftInEndPit, int expectedStonesInSouthAwari)
        {
            var board = new AwariBoard(
                a: 0, b: 4, c: 4, d: stonesInEndPit, e: 4, f: 4,
                A: 4, B: 4, C: 4, D: 4, E: 4, F: 4,
                southAwari: 0,
                northAwari: 8 - stonesInEndPit);

            board.Sow("F");

            board.Pits["d"].Should().Be(expectedStonesLeftInEndPit);
            board.SouthAwari.Should().Be(expectedStonesInSouthAwari);
        }
    }
}