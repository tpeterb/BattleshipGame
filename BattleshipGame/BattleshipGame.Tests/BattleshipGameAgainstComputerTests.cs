using System;
using System.Collections.Generic;
using System.Text;
using BattleshipGame.Model;
using Microsoft.VisualStudio.TestTools.UnitTesting;

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
                       })
                },
                new List<Ship>
                {
                    new Ship(
                        ShipType.Cruiser,
                        new List<Position>
                        {
                            new Position(5, 6),
                            new Position(6, 6),
                        })
                });

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
                       })
                },
                new List<Ship>
                {
                    new Ship(
                        ShipType.Cruiser,
                        new List<Position>
                        {
                            new Position(5, 6),
                            new Position(6, 6),
                        })
                });

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

        [TestMethod]
        public void CreateComputerShot_ThePreviousComputerShotHitAShipButAllAdjacentSquaresHaveAlreadyBeenGuessed_CreatesARandomTarget()
        {
            // Arrange

            Position lastComputerTarget = new Position(8, 6);

            Position leftNeighbour = new Position(8, 5);
            Position rightNeighbour = new Position(8, 7);
            Position topNeighbour = new Position(7, 6);
            Position bottomNeighbour = new Position(9, 6);

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
                       })
                },
                new List<Ship>
                {
                    new Ship(
                        ShipType.Cruiser,
                        new List<Position>
                        {
                            new Position(5, 6),
                            new Position(6, 6),
                        })
                });

            underTest.PlayerOneHits = 0;
            underTest.PlayerTwoHits = 2;
            underTest.NumberOfTurns = 11;

            underTest.PlayerNameToMove = underTest.PlayerTwo.PlayerName;

            underTest.PlayerOneGuesses.Add(new Position(0, 0));
            underTest.PlayerOneGuesses.Add(new Position(0, 1));
            underTest.PlayerOneGuesses.Add(new Position(0, 2));
            underTest.PlayerOneGuesses.Add(new Position(0, 3));
            underTest.PlayerOneGuesses.Add(new Position(0, 4));
            underTest.PlayerOneGuesses.Add(new Position(0, 5));
            underTest.PlayerTwoGuesses.Add(leftNeighbour);
            underTest.PlayerTwoGuesses.Add(rightNeighbour);
            underTest.PlayerTwoGuesses.Add(bottomNeighbour);
            underTest.PlayerTwoGuesses.Add(topNeighbour);
            underTest.PlayerTwoGuesses.Add(lastComputerTarget);

            underTest.SinkingAtPreviousHitOfPlayerTwo = true;

            // Act

            Position actual = underTest.CreateComputerShot();

            // Assert

            Console.WriteLine(Convert.ToString(actual.Row) + ", " + Convert.ToString(actual.Column));
            Console.WriteLine(Convert.ToString(lastComputerTarget.Row) + ", " + Convert.ToString(lastComputerTarget.Column));
            Assert.AreNotEqual(lastComputerTarget, actual);
            Assert.IsFalse(actual.Equals(topNeighbour));
            Assert.IsFalse(actual.Equals(rightNeighbour));
            Assert.IsFalse(actual.Equals(leftNeighbour));
            Assert.IsFalse(actual.Equals(bottomNeighbour));
            Assert.IsFalse(underTest.PlayerTwoGuesses.Contains(actual));
        }

        [TestMethod]
        public void CreateStartingPositionForComputer_CalledAtTheBeginningOfTheGame_CreatesAValidStartingPosition()
        {
            // Arrange

            var battleshipGame = new BattleshipGameWithTwoPlayers(
                new Player(PlayerType.Human, "Pisti"),
                new Player(PlayerType.Human, "Jancsi"),
                new List<Ship>
                {
                    new Ship(
                        ShipType.AircraftCarrier,
                        new List<Position>
                        {
                            new Position(0, 0),
                            new Position(0, 1),
                            new Position(0, 2),
                            new Position(0, 3),
                            new Position(0, 4)
                        }),
                    new Ship(
                        ShipType.Battleship,
                        new List<Position>
                        {
                            new Position(2, 6),
                            new Position(3, 6),
                            new Position(4, 6),
                            new Position(5, 6)
                        }),
                    new Ship(
                        ShipType.Submarine,
                        new List<Position>
                        {
                            new Position(8, 2),
                            new Position(8, 3),
                            new Position(8, 4)
                        }),
                    new Ship(
                        ShipType.Cruiser,
                        new List<Position>
                        {
                            new Position(0, 9),
                            new Position(1, 9),
                            new Position(2, 9)
                        }),
                    new Ship(
                        ShipType.Destroyer,
                        new List<Position>
                        {
                            new Position(4, 1),
                            new Position(4, 2)
                        })
                },
                new List<Ship>());

            // Act

            List<Ship> actual = BattleshipGameAgainstComputer.CreateStartingPositionForComputer();
            battleshipGame.PlayerTwoCurrentShips = actual;
            battleshipGame.PlayerTwoOriginalShips = actual;

            // Assert

            Assert.AreEqual(5, actual.Count);
            Assert.IsNotNull(actual);
        }
    }
}
