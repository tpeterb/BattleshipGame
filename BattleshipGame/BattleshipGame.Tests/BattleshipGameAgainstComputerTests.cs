using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using BattleshipGame.Model;

namespace BattleshipGame.Tests
{
    [TestClass]
    public class BattleshipGameAgainstComputerTests
    {

        [TestMethod]
        public void CreateComputerShot_ThePreviousComputerShotDidNotHitAnything_CreatesARandomTarget()
        {
            // Arrange

            var underTest = new BattleshipGameAgainstComputer(
                new Player(PlayerType.Human, "Attila"),
                new List<Ship>
                {
                    new Ship(
                       ShipType.Battleship,
                       new List<Position>
                       {
                           new Position(2, 4),
                           new Position(2, 5),
                           new Position(2, 6),
                           new Position(2, 7),
                       }
                    )
                },
                new List<Ship>
                {
                    new Ship(
                        ShipType.Cruiser,
                        new List<Position>
                        {
                            new Position(5, 6),
                            new Position(6, 6),
                        }
                    )
                }
            );

            underTest.PlayerNameToMove = underTest.PlayerTwo.PlayerName;

            underTest.PlayerOneHits = 1;
            underTest.PlayerTwoHits = 0;
            underTest.NumberOfTurns = 2;

            underTest.PlayerOneGuesses.Add(new Position(7, 6));
            underTest.PlayerOneGuesses.Add(new Position(2, 8));
            underTest.PlayerTwoGuesses.Add(new Position(0, 6));

            underTest.SinkingAtPreviousHitOfPlayerTwo = false;

            // Act

            var actual = underTest.CreateComputerShot();

            // Assert

            Assert.IsTrue(Model.BattleshipGame.IsPositionValid(actual));
            Assert.IsFalse(underTest.PlayerTwoGuesses.Contains(actual));

        }

        [TestMethod]
        public void CreateComputerShot_ThePreviousComputerShotHitAShip_CreatesATargetNextToThePreviousShot()
        {
            // Arrange

            Position lastComputerTarget = new Position(2, 7);

            var underTest = new BattleshipGameAgainstComputer(
                new Player(PlayerType.Human, "Attila"),
                new List<Ship>
                {
                    new Ship(
                       ShipType.Battleship,
                       new List<Position>
                       {
                           new Position(2, 4),
                           new Position(2, 5),
                           new Position(2, 6),
                       }
                    )
                },
                new List<Ship>
                {
                    new Ship(
                        ShipType.Cruiser,
                        new List<Position>
                        {
                            new Position(5, 6),
                            new Position(6, 6),
                        }
                    )
                }
            );

            underTest.PlayerNameToMove = underTest.PlayerTwo.PlayerName;

            underTest.PlayerOneHits = 1;
            underTest.PlayerTwoHits = 1;
            underTest.NumberOfTurns = 2;

            underTest.PlayerOneGuesses.Add(new Position(7, 6));
            underTest.PlayerOneGuesses.Add(new Position(5, 3));
            underTest.PlayerTwoGuesses.Add(lastComputerTarget);

            underTest.SinkingAtPreviousHitOfPlayerTwo = true;

            // Act

            var actual = underTest.CreateComputerShot();

            // Assert

            Assert.IsTrue(Model.BattleshipGame.IsPositionValid(actual));
            Assert.IsFalse(underTest.PlayerTwoGuesses.Contains(actual));
            Assert.IsTrue(actual.Equals(new Position(lastComputerTarget.Row + 1, lastComputerTarget.Column))
                || actual.Equals(new Position(lastComputerTarget.Row - 1, lastComputerTarget.Column))
                || actual.Equals(new Position(lastComputerTarget.Row, lastComputerTarget.Column - 1))
                || actual.Equals(new Position(lastComputerTarget.Row, lastComputerTarget.Column + 1)));

        }

    }
}
