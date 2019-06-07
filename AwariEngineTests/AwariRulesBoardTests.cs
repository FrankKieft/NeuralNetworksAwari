using AwariEngine;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Linq;

namespace AwariEngineTests
{
    /// <summary>
    /// All rules based on http://web.cecs.pdx.edu/~bart/cs510games-summer2000/hw4/bart-awari-rules.html
    /// </summary>
    [TestClass]
    public class AwariRulesBoardTests
    {
        private AwariBoard _initialBoard;
                
        [TestInitialize]
        public void Initialize()
        {
            _initialBoard = AwariBoard.GetInitialBoard();
        }
        
        [TestMethod]
        public void The_initial_configuration_consists_of_12_pits_into_which_a_number_of_stones_are_placed()
        {
            _initialBoard.Pits.Count.Should().Be(12);
        }

        [TestMethod]
        public void Starting_from_the_NE_corner_and_moving_counterclockwise_the_pits_are_named_by_the_letters_a_through_f_first_in_lowercase_and_then_in_uppercase()
        {
            var expectedKeysNorth = new[] { "f", "e", "d", "c", "b", "a" };
            var expectedKeysSouth = new[] { "A", "B", "C", "D", "E", "F" };

            var i = 0;
            foreach (var pit in _initialBoard.Pits)
            {
                var expectedKey = i < 6 ? expectedKeysSouth[i] : expectedKeysNorth[11-i] ;
                i++;
                pit.Key.Should().Be(expectedKey);
            }
        }
        
       [TestMethod]
        public void Two_end_pits_contain_stones_captured_by_the_two_sides_during_play()
        {
            _initialBoard.NorthAwari.Should().Be(0);
            _initialBoard.SouthAwari.Should().Be(0);
        }

        [TestMethod]
        public void Each_pit_initially_contains_4_stones()
        {
            _initialBoard.Pits.Any(x => x.Value != 4).Should().BeFalse();
        }

        [TestMethod]
        public void The_board_starts_with_48_stones()
        {
            _initialBoard.TotalStones.Should().Be(48);
        }

        [TestMethod]
        public void All_48_stones_must_always_be_somewhere()
        {
            // Arrange
            Action validPosition = () => new AwariBoard(3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 6, 6);
            Action invalidPosition = () => new AwariBoard(10, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 6, 6);

            // Act & Assert
            validPosition.Should().NotThrow<Exception>();
            invalidPosition.Should().Throw<ArgumentException>().WithMessage("AwariBoard should always contain 48 stones.");
        }
    }
}
