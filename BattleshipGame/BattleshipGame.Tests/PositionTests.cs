using System;
using System.Collections.Generic;
using System.Text;
using BattleshipGame.Model;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BattleshipGame.Tests
{
    [TestClass]
    public class PositionTests
    {
        [DataTestMethod]
        [DataRow(2, 5)]
        [DataRow(8, 4)]
        [DataRow(1, 7)]
        [DataRow(5, 2)]
        [DataRow(1, 8)]
        [DataRow(7, 4)]
        [DataRow(9, 6)]
        [DataRow(0, 0)]
        [DataRow(2, 7)]
        [DataRow(3, 5)]
        public void GetPositionInDirection_WithValidPositions_ReturnsTheAppropriatePositions(int row, int col)
        {
            // Arrange

            Position underTest = new Position(row, col);

            Position expectedPositionAbove = new Position(row - 1, col);
            Position expectedPositionBelow = new Position(row + 1, col);
            Position expectedPositionToTheLeft = new Position(row, col - 1);
            Position expectedPositionToTheRight = new Position(row, col + 1);

            // Act

            Position actualPositionAbove = underTest.GetPositionInDirection(Direction.Up);
            Position actualPositionBelow = underTest.GetPositionInDirection(Direction.Down);
            Position actualPositionToTheRight = underTest.GetPositionInDirection(Direction.Right);
            Position actualPositionToTheLeft = underTest.GetPositionInDirection(Direction.Left);

            // Assert

            Assert.AreEqual(expectedPositionAbove, actualPositionAbove);
            Assert.AreEqual(expectedPositionBelow, actualPositionBelow);
            Assert.AreEqual(expectedPositionToTheLeft, actualPositionToTheLeft);
            Assert.AreEqual(expectedPositionToTheRight, actualPositionToTheRight);
        }
    }
}
