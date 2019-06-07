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
            _initialBoard.Move(2);
            _initialBoard.FirstToMove.Should().Be(Player.North);
        }
    }
}