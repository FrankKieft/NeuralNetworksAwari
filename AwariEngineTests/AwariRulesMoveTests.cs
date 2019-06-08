using AwariEngine;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AwariEngineTests
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
        public void The_south_player_moves_first()
        {
            _initialBoard.FirstToMove.Should().Be(Player.South);
        }

        [TestMethod]
        public void The_players_alternate_moves()
        {
            _initialBoard.Sow("B");
            _initialBoard.FirstToMove.Should().Be(Player.North);
        }

        [TestMethod]
        public void A_pit_may_be_sown_if_it_contains_one_or_more_stones()
        {
            var board = new AwariBoard(3, 0, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 6, 9);

            board.CanSow("A").Should().BeTrue();
            board.CanSow("B").Should().BeFalse();
            board.CanSow("C").Should().BeTrue();
        }

        [TestMethod]
        public void The_stones_are_removed_from_the_pit_and_placed_one_at_a_time_into_subsequent_pits_moving_around_the_board_counterclockwise_()
        {
            _initialBoard.Sow("D");
            _initialBoard.Pits["D"].Should().Be(0);
            _initialBoard.Pits["E"].Should().Be(5);
            _initialBoard.Pits["F"].Should().Be(5);
            _initialBoard.Pits["a"].Should().Be(5);
            _initialBoard.Pits["b"].Should().Be(5);
            _initialBoard.Pits["c"].Should().Be(4);
        }

        [TestMethod]
        public void The_original_pit_is_skipped_whenever_it_is_encountered()
        {
            var board = new AwariBoard(3, 0, 3, 3, 15, 3, 3, 3, 3, 3, 3, 3, 1, 2);
            board.Sow("E");
            board.Pits["E"].Should().Be(0);
        }
    }
}