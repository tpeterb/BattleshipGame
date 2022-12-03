using Microsoft.VisualStudio.TestTools.UnitTesting;
using BattleshipGame.Model;
using System.Collections.Generic;
using System;
using System.Linq;

namespace BattleshipGame.Tests
{
    [TestClass]
    public class BattleshipGameUnitTests
    {

        /* A valid starting position:
         
        var battleshipGame = new BattleshipGameWithTwoPlayers(
                new Player(PlayerType.Human, "Pisti"),
                new Player(PlayerType.Human, "Jancsi"),
                new List<Ship> {
                    new Ship(
                        ShipType.AircraftCarrier,
                        new List<Position> {
                            new Position(0, 0),
                            new Position(0, 1),
                            new Position(0, 2),
                            new Position(0, 3),
                            new Position(0, 4)
                        }),
                    new Ship(
                        ShipType.Battleship,
                        new List<Position> {
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
                        }
                    ),
                    new Ship(
                        ShipType.Cruiser,
                        new List<Position>
                        {
                            new Position(0, 9),
                            new Position(1, 9),
                            new Position(2, 9)
                        }
                    ),
                    new Ship(
                        ShipType.Destroyer,
                        new List<Position>
                        {
                            new Position(4, 1),
                            new Position(4, 2)
                        }
                    )
                },
                new List<Ship>
                {
                    new Ship(
                        ShipType.AircraftCarrier,
                        new List<Position>
                        {
                            new Position(2, 6),
                            new Position(3, 6),
                            new Position(4, 6),
                            new Position(5, 6),
                            new Position(6, 6),
                        }
                    ),
                    new Ship(
                        ShipType.Battleship,
                        new List<Position>
                        {
                            new Position(7, 1),
                            new Position(7, 2),
                            new Position(7, 3),
                            new Position(7, 4),
                        }
                    ),
                    new Ship(
                        ShipType.Submarine,
                        new List<Position>
                        {
                            new Position(1, 2),
                            new Position(1, 3),
                            new Position(1, 4)
                        }
                    ),
                    new Ship(
                        ShipType.Cruiser,
                        new List<Position>
                        {
                            new Position(7, 8),
                            new Position(8, 8),
                            new Position(9, 8),
                        }
                    ),
                    new Ship(
                        ShipType.Destroyer,
                        new List<Position>
                        {
                            new Position(4, 8),
                            new Position(4, 9)
                        }
                    )
                }
            ); 

         */

        [TestMethod]
        public void IsGameOver_WithWinningStateForPlayerOneInTwoPlayerMode_ReturnsTrue()
        {
            // Arrange

            var battleshipGame = new BattleshipGameWithTwoPlayers(
                new Player(PlayerType.Human, "Pisti"),
                new Player(PlayerType.Human, "Jancsi"),
                new List<Ship> {
                    new Ship(
                        ShipType.AircraftCarrier,
                        new List<Position> {
                            new Position(0, 0),
                            new Position(0, 1),
                            new Position(0, 2)
                        }),
                    new Ship(
                        ShipType.Battleship,
                        new List<Position> {
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
                            new Position(8, 3)
                        }
                    )
                },
                new List<Ship>{ }
            );

            bool expected = true;

            // Act

            bool actual = battleshipGame.IsGameOver();

            // Assert

            Assert.AreEqual(expected, actual);
            Assert.IsTrue(actual);

        }

        [TestMethod]
        public void IsGameOver_WithWinningPositionForTheSecondPlayer_ReturnsTrue()
        {
            // Arrange

            var battleshipGame = new BattleshipGameWithTwoPlayers(
                new Player(PlayerType.Human, "Pisti"),
                new Player(PlayerType.Human, "Jancsi"),
                new List<Ship> { },
                new List<Ship>
                {
                    new Ship(
                        ShipType.Cruiser,
                        new List<Position>
                        {
                            new Position(8, 8),
                            new Position(9, 8),
                        }
                    ),
                    new Ship(
                        ShipType.Destroyer,
                        new List<Position>
                        {
                            new Position(4, 8)
                        }
                    )
                }
            );

            bool expected = true;

            // Act

            bool actual = battleshipGame.IsGameOver();

            // Assert

            Assert.AreEqual(expected, actual);
            Assert.IsTrue(actual);
        }

        [TestMethod]
        public void IsGameOver_WithANonWinningPositionForBothPlayers_ReturnsFalse()
        {
            // Arrange

            var battleshipGame = new BattleshipGameWithTwoPlayers(
                new Player(PlayerType.Human, "Pisti"),
                new Player(PlayerType.Human, "Jancsi"),
                new List<Ship> {
                    new Ship(
                        ShipType.Destroyer,
                        new List<Position>
                        {
                            new Position(4, 1)
                        }
                    )
                },
                new List<Ship>
                {
                    new Ship(
                        ShipType.Battleship,
                        new List<Position>
                        {
                            new Position(7, 1),
                            new Position(7, 2),
                            new Position(7, 3)
                        }
                    ),
                }
            );

            bool expected = false;

            // Act

            bool actual = battleshipGame.IsGameOver();

            // Assert

            Assert.AreEqual(expected, actual);
            Assert.IsFalse(actual);
        }

        [TestMethod]
        public void IsPlayerOneWinning_WithWinningPositionForPlayerOne_ReturnsTrue()
        {
            // Arrange

            var battleshipGame = new BattleshipGameWithTwoPlayers(
                new Player(PlayerType.Human, "Pisti"),
                new Player(PlayerType.Human, "Jancsi"),
                new List<Ship> {
                    new Ship(
                        ShipType.AircraftCarrier,
                        new List<Position> {
                            new Position(0, 0),
                        }),
                    new Ship(
                        ShipType.Battleship,
                        new List<Position> {
                            new Position(2, 6),
                        }),
                    new Ship(
                        ShipType.Submarine,
                        new List<Position>
                        {
                            new Position(8, 2),
                        }
                    )
                },
                new List<Ship> { }
            );

            bool expected = true;

            // Act

            bool actual = battleshipGame.IsPlayerOneWinning();

            // Assert

            Assert.AreEqual(expected, actual);
            Assert.IsTrue(actual);
        }

        [TestMethod]
        public void IsPlayerOneWinning_WithNonWinningPositionForPlayerOne_ReturnsFalse()
        {
            // Arrange

            var battleshipGame = new BattleshipGameWithTwoPlayers(
                new Player(PlayerType.Human, "Pisti"),
                new Player(PlayerType.Human, "Jancsi"),
                new List<Ship> {
                    new Ship(
                        ShipType.Submarine,
                        new List<Position>
                        {
                            new Position(8, 2),
                        }
                    ),
                    new Ship(
                        ShipType.Cruiser,
                        new List<Position>
                        {
                            new Position(0, 9),
                            new Position(1, 9),
                        }
                    ),
                    new Ship(
                        ShipType.Destroyer,
                        new List<Position>
                        {
                            new Position(4, 1),
                            new Position(4, 2)
                        }
                    )
                },
                new List<Ship>
                {
                    new Ship(
                        ShipType.AircraftCarrier,
                        new List<Position>
                        {
                            new Position(2, 6),
                        }
                    ),
                    new Ship(
                        ShipType.Battleship,
                        new List<Position>
                        {
                            new Position(7, 1),
                            new Position(7, 2),
                            new Position(7, 3),
                        }
                    ),
                    new Ship(
                        ShipType.Submarine,
                        new List<Position>
                        {
                            new Position(1, 2),
                            new Position(1, 3),
                        }
                    ),
                    new Ship(
                        ShipType.Destroyer,
                        new List<Position>
                        {
                            new Position(4, 8),
                            new Position(4, 9)
                        }
                    )
                }
            );

            bool expected = false;

            // Act

            bool actual = battleshipGame.IsPlayerOneWinning();

            // Assert

            Assert.AreEqual(expected, actual);
            Assert.IsFalse(actual);
        }

        [TestMethod]
        public void IsPlayerTwoWinning_WithWinningPositionForPlayerTwo_ReturnsTrue()
        {
            // Arrange

            var battleshipGame = new BattleshipGameWithTwoPlayers(
                new Player(PlayerType.Human, "Pisti"),
                new Player(PlayerType.Human, "Jancsi"),
                new List<Ship> { },
                new List<Ship>
                {
                    new Ship(
                        ShipType.AircraftCarrier,
                        new List<Position>
                        {
                            new Position(2, 6),
                            new Position(3, 6),
                        }
                    )
                }
            );

            bool expected = true;

            // Act

            bool actual = battleshipGame.IsPlayerTwoWinning();

            // Assert

            Assert.AreEqual(expected, actual);
            Assert.IsTrue(actual);
        }

        [TestMethod]
        public void IsPlayerTwoWinning_WithNonWinningPositionForPlayerTwo_ReturnsFalse()
        {
            // Arrange

            var battleshipGame = new BattleshipGameWithTwoPlayers(
                new Player(PlayerType.Human, "Pisti"),
                new Player(PlayerType.Human, "Jancsi"),
                new List<Ship> {
                    new Ship(
                        ShipType.AircraftCarrier,
                        new List<Position> {
                            new Position(0, 0),
                        }),
                    new Ship(
                        ShipType.Battleship,
                        new List<Position> {
                            new Position(2, 6),
                            new Position(3, 6),
                            new Position(4, 6),
                        }),
                    new Ship(
                        ShipType.Cruiser,
                        new List<Position>
                        {
                            new Position(0, 9),
                            new Position(1, 9),
                        }
                    ),
                    new Ship(
                        ShipType.Destroyer,
                        new List<Position>
                        {
                            new Position(4, 1),
                            new Position(4, 2)
                        }
                    )
                },
                new List<Ship>
                {
                    new Ship(
                        ShipType.Battleship,
                        new List<Position>
                        {
                            new Position(7, 1),
                        }
                    ),
                    new Ship(
                        ShipType.Submarine,
                        new List<Position>
                        {
                            new Position(1, 2),
                            new Position(1, 3),
                        }
                    ),
                    new Ship(
                        ShipType.Destroyer,
                        new List<Position>
                        {
                            new Position(4, 8),
                            new Position(4, 9)
                        }
                    )
                }
            );

            bool expected = false;

            // Act

            bool actual = battleshipGame.IsPlayerTwoWinning();

            // Assert

            Assert.AreEqual(expected, actual);
            Assert.IsFalse(actual);
        }

        [DataRow(0, 0)]
        [DataRow(9, 9)]
        [DataRow(2, 4)]
        [DataRow(0, 5)]
        [DataRow(8, 0)]
        [DataRow(3, 5)]
        [DataRow(4, 7)]
        [DataTestMethod]
        public void IsPositionValid_WithValidPosition_ReturnsTrue(int row, int col)
        {
            // Arrange

            Position position = new Position(row, col);

            bool expected = true;

            // Act

            bool actual = BattleshipGame.Model.BattleshipGame.IsPositionValid(position);

            // Assert

            Assert.AreEqual(expected, actual);
            Assert.IsTrue(actual);

        }

        [DataTestMethod]
        [DataRow(-1, 0)]
        [DataRow(3, -4)]
        [DataRow(-13, -6)]
        [DataRow(10, 0)]
        [DataRow(5, 12)]
        [DataRow(36, 25)]
        public void IsPositionValid_WithInvalidPosition_ReturnsFalse(int row, int col)
        {
            // Arrange

            Position position = new Position(row, col);

            bool expected = false;

            // Act

            bool actual = BattleshipGame.Model.BattleshipGame.IsPositionValid(position);

            // Assert

            Assert.AreEqual(expected, actual);
            Assert.IsFalse(actual);

        }

        [TestMethod]
        public void MakeShot_WithInvalidPosition_ThrowsArgumentException()
        {
            // Arrange

            Player playerToShoot = new Player(PlayerType.Human, "Pisti");
            Position positionToShootAt = new Position(4, 11);

            var battleshipGame = new BattleshipGameWithTwoPlayers(
                playerToShoot,
                new Player(PlayerType.Human, "Jancsi"),
                new List<Ship> {
                    new Ship(
                        ShipType.AircraftCarrier,
                        new List<Position> {
                            new Position(0, 0),
                        }),
                    new Ship(
                        ShipType.Battleship,
                        new List<Position> {
                            new Position(2, 6),
                            new Position(3, 6),
                            new Position(4, 6),
                        }),
                    new Ship(
                        ShipType.Cruiser,
                        new List<Position>
                        {
                            new Position(0, 9),
                            new Position(1, 9),
                        }
                    ),
                    new Ship(
                        ShipType.Destroyer,
                        new List<Position>
                        {
                            new Position(4, 1),
                            new Position(4, 2)
                        }
                    )
                },
                new List<Ship>
                {
                    new Ship(
                        ShipType.Battleship,
                        new List<Position>
                        {
                            new Position(7, 1),
                        }
                    ),
                    new Ship(
                        ShipType.Submarine,
                        new List<Position>
                        {
                            new Position(1, 2),
                            new Position(1, 3),
                        }
                    ),
                    new Ship(
                        ShipType.Destroyer,
                        new List<Position>
                        {
                            new Position(4, 8),
                            new Position(4, 9)
                        }
                    )
                }
            );

            // Act, Assert

            Assert.ThrowsException<ArgumentException>(() => battleshipGame.MakeShot(playerToShoot, positionToShootAt),
                "Shooting at a position that's not present on the board!");
        }

        [TestMethod]
        public void MakeShot_CannotMakeShotDueToTryingToShootingWithTheWrongPlayer_DoesNotMakeTheShot()
        {
            // Arrange

            Player playerToTryToShootWith = new Player(PlayerType.Human, "Jancsi");

            Position positionToShootAt = new Position(8, 2);

            var underTest = new BattleshipGameWithTwoPlayers(
                new Player(PlayerType.Human, "Pisti"),
                playerToTryToShootWith,
                new List<Ship> {
                    new Ship(
                        ShipType.Submarine,
                        new List<Position>
                        {
                            new Position(8, 2),
                        }
                    ),
                    new Ship(
                        ShipType.Cruiser,
                        new List<Position>
                        {
                            new Position(0, 9),
                            new Position(1, 9),
                        }
                    ),
                    new Ship(
                        ShipType.Destroyer,
                        new List<Position>
                        {
                            new Position(4, 1),
                            new Position(4, 2)
                        }
                    )
                },
                new List<Ship>
                {
                    new Ship(
                        ShipType.AircraftCarrier,
                        new List<Position>
                        {
                            new Position(2, 6),
                        }
                    ),
                    new Ship(
                        ShipType.Battleship,
                        new List<Position>
                        {
                            new Position(7, 1),
                            new Position(7, 2),
                            new Position(7, 3),
                        }
                    ),
                    new Ship(
                        ShipType.Submarine,
                        new List<Position>
                        {
                            new Position(1, 2),
                            new Position(1, 3),
                        }
                    ),
                    new Ship(
                        ShipType.Destroyer,
                        new List<Position>
                        {
                            new Position(4, 8),
                            new Position(4, 9)
                        }
                    )
                }
            );

            var expected = new BattleshipGameWithTwoPlayers(
                new Player(PlayerType.Human, "Pisti"),
                playerToTryToShootWith,
                new List<Ship> {
                    new Ship(
                        ShipType.Submarine,
                        new List<Position>
                        {
                            new Position(8, 2),
                        }
                    ),
                    new Ship(
                        ShipType.Cruiser,
                        new List<Position>
                        {
                            new Position(0, 9),
                            new Position(1, 9),
                        }
                    ),
                    new Ship(
                        ShipType.Destroyer,
                        new List<Position>
                        {
                            new Position(4, 1),
                            new Position(4, 2)
                        }
                    )
                },
                new List<Ship>
                {
                    new Ship(
                        ShipType.AircraftCarrier,
                        new List<Position>
                        {
                            new Position(2, 6),
                        }
                    ),
                    new Ship(
                        ShipType.Battleship,
                        new List<Position>
                        {
                            new Position(7, 1),
                            new Position(7, 2),
                            new Position(7, 3),
                        }
                    ),
                    new Ship(
                        ShipType.Submarine,
                        new List<Position>
                        {
                            new Position(1, 2),
                            new Position(1, 3),
                        }
                    ),
                    new Ship(
                        ShipType.Destroyer,
                        new List<Position>
                        {
                            new Position(4, 8),
                            new Position(4, 9)
                        }
                    )
                }
            );

            // Act

            underTest.MakeShot(playerToTryToShootWith, positionToShootAt);

            // Assert

            Assert.AreEqual(expected, underTest);
        }

        [TestMethod]
        public void MakeShot_CannotMakeShotWithPlayerOneBecauseTheGivenPositionWasAlreadyShotAt_DoesNotMakeShot()
        {
            // Arrange

            Player playerToShootWith = new Player(PlayerType.Human, "Pisti");

            Position positionToShootAt = new Position(9, 6);

            var underTest = new BattleshipGameWithTwoPlayers(
                playerToShootWith,
                new Player(PlayerType.Human, "Jancsi"),
                new List<Ship> {
                    new Ship(
                        ShipType.Destroyer,
                        new List<Position>
                        {
                            new Position(4, 1)
                        }
                    )
                },
                new List<Ship>
                {
                    new Ship(
                        ShipType.Battleship,
                        new List<Position>
                        {
                            new Position(7, 1),
                            new Position(7, 2),
                            new Position(7, 3)
                        }
                    ),
                }
            );

            underTest.PlayerOneGuesses.Add(new Position(5, 5));
            underTest.PlayerOneGuesses.Add(new Position(1, 4));
            underTest.PlayerOneGuesses.Add(positionToShootAt);
            underTest.PlayerTwoGuesses.Add(new Position(2, 2));
            underTest.PlayerTwoGuesses.Add(new Position(2, 3));

            var expected = new BattleshipGameWithTwoPlayers(
                playerToShootWith,
                new Player(PlayerType.Human, "Jancsi"),
                new List<Ship> {
                    new Ship(
                        ShipType.Destroyer,
                        new List<Position>
                        {
                            new Position(4, 1)
                        }
                    )
                },
                new List<Ship>
                {
                    new Ship(
                        ShipType.Battleship,
                        new List<Position>
                        {
                            new Position(7, 1),
                            new Position(7, 2),
                            new Position(7, 3)
                        }
                    ),
                }
            );

            expected.PlayerOneGuesses.Add(new Position(5, 5));
            expected.PlayerOneGuesses.Add(new Position(1, 4));
            expected.PlayerOneGuesses.Add(positionToShootAt);
            expected.PlayerTwoGuesses.Add(new Position(2, 2));
            expected.PlayerTwoGuesses.Add(new Position(2, 3));

            // Act

            underTest.MakeShot(playerToShootWith, positionToShootAt);

            // Assert

            Assert.AreEqual(expected, underTest);
        }

        [TestMethod]
        public void MakeShot_CannotMakeShotWithPlayerTwoBecauseTheGivenPositionWasAlreadyShotAt_DoesNotMakeShot()
        {
            // Arrange

            Player playerToShootWith = new Player(PlayerType.Human, "Jancsi");

            Position positionToShootAt = new Position(2, 3);

            var underTest = new BattleshipGameWithTwoPlayers(
                new Player(PlayerType.Human, "Pisti"),
                playerToShootWith,
                new List<Ship> {
                    new Ship(
                        ShipType.AircraftCarrier,
                        new List<Position>
                        {
                            new Position(4, 1),
                            new Position(4, 2),
                            new Position(4, 3),
                        }
                    ),
                    new Ship(
                        ShipType.Destroyer,
                        new List<Position> 
                        {
                            new Position(8, 1),
                            new Position(9, 1)
                        })
                },
                new List<Ship>
                {
                    new Ship(
                        ShipType.Cruiser,
                        new List<Position>
                        {
                            new Position(5, 4),
                            new Position(6, 4),
                            new Position(7, 4)
                        }
                    ),
                }
            );

            underTest.PlayerNameToMove = underTest.PlayerTwo.PlayerName;

            underTest.PlayerOneGuesses.Add(new Position(5, 5));
            underTest.PlayerOneGuesses.Add(new Position(1, 4));
            underTest.PlayerTwoGuesses.Add(positionToShootAt);
            underTest.PlayerTwoGuesses.Add(new Position(2, 4));

            var expected = new BattleshipGameWithTwoPlayers(
                new Player(PlayerType.Human, "Pisti"),
                playerToShootWith,
                new List<Ship> {
                    new Ship(
                        ShipType.AircraftCarrier,
                        new List<Position>
                        {
                            new Position(4, 1),
                            new Position(4, 2),
                            new Position(4, 3),
                        }
                    ),
                    new Ship(
                        ShipType.Destroyer,
                        new List<Position>
                        {
                            new Position(8, 1),
                            new Position(9, 1)
                        })
                },
                new List<Ship>
                {
                    new Ship(
                        ShipType.Cruiser,
                        new List<Position>
                        {
                            new Position(5, 4),
                            new Position(6, 4),
                            new Position(7, 4)
                        }
                    ),
                }
            );

            expected.PlayerOneGuesses.Add(new Position(5, 5));
            expected.PlayerOneGuesses.Add(new Position(1, 4));
            expected.PlayerTwoGuesses.Add(positionToShootAt);
            expected.PlayerTwoGuesses.Add(new Position(2, 4));
            expected.PlayerNameToMove = expected.PlayerTwo.PlayerName;

            // Act

            underTest.MakeShot(playerToShootWith, positionToShootAt);

            // Assert

            Assert.AreEqual(expected, underTest);

        }

        [TestMethod]
        public void MakeShot_CanMakeShotWithPlayerOneAndTheShotHitsAShip_MakesTheShot()
        {
            // Arrange

            Player playerToShootWith = new Player(PlayerType.Human, "Pisti");

            Position positionToShootAt = new Position(7, 3);

            var underTest = new BattleshipGameWithTwoPlayers(
                playerToShootWith,
                new Player(PlayerType.Human, "Jancsi"),
                new List<Ship> {
                    new Ship(
                        ShipType.Destroyer,
                        new List<Position>
                        {
                            new Position(4, 1)
                        }
                    )
                },
                new List<Ship>
                {
                    new Ship(
                        ShipType.Battleship,
                        new List<Position>
                        {
                            new Position(7, 1),
                            new Position(7, 2),
                            new Position(7, 3)
                        }
                    ),
                }
            );

            underTest.PlayerOneHits = 3;
            underTest.PlayerTwoHits = 2;
            underTest.NumberOfTurns = 6;

            underTest.PlayerOneGuesses.Add(new Position(1,1));
            underTest.PlayerOneGuesses.Add(new Position(1,2));
            underTest.PlayerOneGuesses.Add(new Position(1,3));
            underTest.PlayerTwoGuesses.Add(new Position(3, 1));
            underTest.PlayerTwoGuesses.Add(new Position(3, 2));
            underTest.PlayerTwoGuesses.Add(new Position(6, 2));

            var expected = new BattleshipGameWithTwoPlayers(
                playerToShootWith,
                new Player(PlayerType.Human, "Jancsi"),
                new List<Ship> {
                    new Ship(
                        ShipType.Destroyer,
                        new List<Position>
                        {
                            new Position(4, 1)
                        }
                    )
                },
                new List<Ship>
                {
                    new Ship(
                        ShipType.Battleship,
                        new List<Position>
                        {
                            new Position(7, 1),
                            new Position(7, 2),
                            new Position(7, 3)
                        }
                    ),
                }
            );

            expected.PlayerTwoCurrentShips.ElementAt(0).ShipPositions.RemoveAt(2);

            expected.PlayerOneGuesses.Add(new Position(1, 1));
            expected.PlayerOneGuesses.Add(new Position(1, 2));
            expected.PlayerOneGuesses.Add(new Position(1, 3));
            expected.PlayerTwoGuesses.Add(new Position(3, 1));
            expected.PlayerTwoGuesses.Add(new Position(3, 2));
            expected.PlayerTwoGuesses.Add(new Position(6, 2));

            expected.PlayerOneGuesses.Add(positionToShootAt);
            expected.PlayerOneHits = 4;
            expected.PlayerTwoHits = 2;
            expected.NumberOfTurns = 7;
            expected.SinkingAtPreviousHitOfPlayerOne = true;
            expected.PlayerNameToMove = expected.PlayerTwo.PlayerName;

            // Act

            underTest.MakeShot(playerToShootWith, positionToShootAt);

            // Assert

            Assert.AreEqual(expected, underTest);

        }

        [TestMethod]
        public void MakeShot_CanMakeShotWithPlayerOneAndTheShotDoesNotHitAShip_MakesTheShot()
        {
            // Arrange

            Player playerToShootWith = new Player(PlayerType.Human, "Pisti");

            Position positionToShootAt = new Position(9, 9);

            var underTest = new BattleshipGameWithTwoPlayers(
                playerToShootWith,
                new Player(PlayerType.Human, "Jancsi"),
                new List<Ship> {
                    new Ship(
                        ShipType.Destroyer,
                        new List<Position>
                        {
                            new Position(4, 1)
                        }
                    )
                },
                new List<Ship>
                {
                    new Ship(
                        ShipType.Battleship,
                        new List<Position>
                        {
                            new Position(7, 1),
                            new Position(7, 2),
                            new Position(7, 3)
                        }
                    ),
                }
            );

            underTest.PlayerOneGuesses.Add(new Position(6, 1));
            underTest.PlayerOneGuesses.Add(new Position(6, 2));
            underTest.PlayerOneGuesses.Add(new Position(6, 3));
            underTest.PlayerOneGuesses.Add(new Position(6, 4));
            underTest.PlayerOneGuesses.Add(new Position(6, 5));
            underTest.PlayerOneGuesses.Add(new Position(0, 0));
            underTest.PlayerOneGuesses.Add(new Position(2, 4));
            underTest.PlayerTwoGuesses.Add(new Position(5, 1));
            underTest.PlayerTwoGuesses.Add(new Position(5, 2));
            underTest.PlayerTwoGuesses.Add(new Position(2, 3));
            underTest.PlayerTwoGuesses.Add(new Position(2, 4));
            underTest.PlayerTwoGuesses.Add(new Position(2, 5));
            underTest.PlayerTwoGuesses.Add(new Position(2, 6));
            underTest.PlayerTwoGuesses.Add(new Position(6, 6));

            underTest.PlayerOneHits = 5;
            underTest.PlayerTwoHits = 5;
            underTest.NumberOfTurns = 14;

            var expected = new BattleshipGameWithTwoPlayers(
                playerToShootWith,
                new Player(PlayerType.Human, "Jancsi"),
                new List<Ship> {
                    new Ship(
                        ShipType.Destroyer,
                        new List<Position>
                        {
                            new Position(4, 1)
                        }
                    )
                },
                new List<Ship>
                {
                    new Ship(
                        ShipType.Battleship,
                        new List<Position>
                        {
                            new Position(7, 1),
                            new Position(7, 2),
                            new Position(7, 3)
                        }
                    ),
                }
            );

            expected.PlayerOneGuesses.Add(new Position(6, 1));
            expected.PlayerOneGuesses.Add(new Position(6, 2));
            expected.PlayerOneGuesses.Add(new Position(6, 3));
            expected.PlayerOneGuesses.Add(new Position(6, 4));
            expected.PlayerOneGuesses.Add(new Position(6, 5));
            expected.PlayerOneGuesses.Add(new Position(0, 0));
            expected.PlayerOneGuesses.Add(new Position(2, 4));
            expected.PlayerTwoGuesses.Add(new Position(5, 1));
            expected.PlayerTwoGuesses.Add(new Position(5, 2));
            expected.PlayerTwoGuesses.Add(new Position(2, 3));
            expected.PlayerTwoGuesses.Add(new Position(2, 4));
            expected.PlayerTwoGuesses.Add(new Position(2, 5));
            expected.PlayerTwoGuesses.Add(new Position(2, 6));
            expected.PlayerTwoGuesses.Add(new Position(6, 6));

            expected.PlayerOneGuesses.Add(positionToShootAt);

            expected.PlayerOneHits = 5;
            expected.PlayerTwoHits = 5;
            expected.NumberOfTurns = 15;
            expected.SinkingAtPreviousHitOfPlayerOne = false;
            expected.PlayerNameToMove = expected.PlayerTwo.PlayerName;

            // Act

            underTest.MakeShot(playerToShootWith, positionToShootAt);

            // Assert

            Assert.AreEqual(expected, underTest);
        }

        [TestMethod]
        public void MakeShot_CanMakeShotWithPlayerTwoAndTheShotHitsAShip_MakesTheShot()
        {
            // Arrange

            Player playerToShootWith = new Player(PlayerType.Human, "Jancsi");

            Position positionToShootAt = new Position(8, 2);

            var underTest = new BattleshipGameWithTwoPlayers(
                new Player(PlayerType.Human, "Pisti"),
                playerToShootWith,
                new List<Ship> {
                    new Ship(
                        ShipType.Submarine,
                        new List<Position>
                        {
                            new Position(8, 2)
                        }
                    ),
                    new Ship(
                        ShipType.Cruiser,
                        new List<Position>
                        {
                            new Position(0, 9),
                            new Position(1, 9),
                        }
                    ),
                    new Ship(
                        ShipType.Destroyer,
                        new List<Position>
                        {
                            new Position(4, 1),
                            new Position(4, 2)
                        }
                    )
                },
                new List<Ship>
                {
                    new Ship(
                        ShipType.AircraftCarrier,
                        new List<Position>
                        {
                            new Position(2, 6)
                        }
                    ),
                    new Ship(
                        ShipType.Battleship,
                        new List<Position>
                        {
                            new Position(7, 1),
                            new Position(7, 2),
                            new Position(7, 3),
                        }
                    ),
                    new Ship(
                        ShipType.Submarine,
                        new List<Position>
                        {
                            new Position(1, 2),
                            new Position(1, 3),
                        }
                    ),
                    new Ship(
                        ShipType.Destroyer,
                        new List<Position>
                        {
                            new Position(4, 8),
                            new Position(4, 9)
                        }
                    )
                }
            );

            underTest.PlayerNameToMove = playerToShootWith.PlayerName;

            underTest.PlayerOneGuesses.Add(new Position(5, 1));
            underTest.PlayerOneGuesses.Add(new Position(5, 2));
            underTest.PlayerOneGuesses.Add(new Position(5, 3));
            underTest.PlayerOneGuesses.Add(new Position(0, 0));
            underTest.PlayerTwoGuesses.Add(new Position(8, 3));
            underTest.PlayerTwoGuesses.Add(new Position(1, 1));
            underTest.PlayerTwoGuesses.Add(new Position(2, 7));

            underTest.PlayerOneHits = 3;
            underTest.PlayerTwoHits = 3;
            underTest.NumberOfTurns = 7;
            
            var expected = new BattleshipGameWithTwoPlayers(
                new Player(PlayerType.Human, "Pisti"),
                playerToShootWith,
                new List<Ship> {
                    new Ship(
                        ShipType.Submarine,
                        new List<Position>
                        {
                            new Position(8, 2)
                        }
                    ),
                    new Ship(
                        ShipType.Cruiser,
                        new List<Position>
                        {
                            new Position(0, 9),
                            new Position(1, 9),
                        }
                    ),
                    new Ship(
                        ShipType.Destroyer,
                        new List<Position>
                        {
                            new Position(4, 1),
                            new Position(4, 2)
                        }
                    )
                },
                new List<Ship>
                {
                    new Ship(
                        ShipType.AircraftCarrier,
                        new List<Position>
                        {
                            new Position(2, 6)
                        }
                    ),
                    new Ship(
                        ShipType.Battleship,
                        new List<Position>
                        {
                            new Position(7, 1),
                            new Position(7, 2),
                            new Position(7, 3),
                        }
                    ),
                    new Ship(
                        ShipType.Submarine,
                        new List<Position>
                        {
                            new Position(1, 2),
                            new Position(1, 3),
                        }
                    ),
                    new Ship(
                        ShipType.Destroyer,
                        new List<Position>
                        {
                            new Position(4, 8),
                            new Position(4, 9)
                        }
                    )
                }
            );

            expected.PlayerOneCurrentShips.ElementAt(0).ShipPositions.RemoveAt(0);
            expected.PlayerOneCurrentShips.ElementAt(0).Destroyed = true;

            expected.PlayerOneGuesses.Add(new Position(5, 1));
            expected.PlayerOneGuesses.Add(new Position(5, 2));
            expected.PlayerOneGuesses.Add(new Position(5, 3));
            expected.PlayerOneGuesses.Add(new Position(0, 0));
            expected.PlayerTwoGuesses.Add(new Position(8, 3));
            expected.PlayerTwoGuesses.Add(new Position(1, 1));
            expected.PlayerTwoGuesses.Add(new Position(2, 7));

            expected.PlayerTwoGuesses.Add(positionToShootAt);

            expected.NumberOfTurns = 8;
            expected.PlayerOneHits = 3;
            expected.PlayerTwoHits = 4;

            expected.SinkingAtPreviousHitOfPlayerTwo = true;

            expected.PlayerNameToMove = expected.PlayerOne.PlayerName;

            // Act

            underTest.MakeShot(playerToShootWith, positionToShootAt);

            // Assert

            Assert.AreEqual(expected, underTest);

        }

        [TestMethod]
        public void MakeShot_CanMakeShotWithPlayerTwoAndTheShotDoesNotHitAShip_MakesTheShot()
        {
            // Arrange

            Player playerToShootWith = new Player(PlayerType.Human, "Jancsi");

            Position positionToShootAt = new Position(6, 8);

            var underTest = new BattleshipGameWithTwoPlayers(
                new Player(PlayerType.Human, "Pisti"),
                playerToShootWith,
                new List<Ship> {
                    new Ship(
                        ShipType.AircraftCarrier,
                        new List<Position> {
                            new Position(0, 0),
                        }),
                    new Ship(
                        ShipType.Battleship,
                        new List<Position> {
                            new Position(2, 6),
                            new Position(3, 6),
                            new Position(4, 6),
                        }),
                    new Ship(
                        ShipType.Cruiser,
                        new List<Position>
                        {
                            new Position(0, 9),
                            new Position(1, 9),
                        }
                    ),
                    new Ship(
                        ShipType.Destroyer,
                        new List<Position>
                        {
                            new Position(4, 1),
                            new Position(4, 2)
                        }
                    )
                },
                new List<Ship>
                {
                    new Ship(
                        ShipType.Battleship,
                        new List<Position>
                        {
                            new Position(7, 1),
                        }
                    ),
                    new Ship(
                        ShipType.Submarine,
                        new List<Position>
                        {
                            new Position(1, 2),
                            new Position(1, 3),
                        }
                    ),
                    new Ship(
                        ShipType.Destroyer,
                        new List<Position>
                        {
                            new Position(4, 8),
                            new Position(4, 9)
                        }
                    )
                }
            );

            underTest.PlayerNameToMove = underTest.PlayerTwo.PlayerName;

            underTest.PlayerOneHits = 2;
            underTest.PlayerTwoHits = 1;
            underTest.NumberOfTurns = 5;

            underTest.PlayerOneGuesses.Add(new Position(1, 1));
            underTest.PlayerOneGuesses.Add(new Position(7, 2));
            underTest.PlayerOneGuesses.Add(new Position(2, 1));
            underTest.PlayerTwoGuesses.Add(new Position(1, 0));
            underTest.PlayerTwoGuesses.Add(new Position(5, 5));

            var expected = new BattleshipGameWithTwoPlayers(
                new Player(PlayerType.Human, "Pisti"),
                playerToShootWith,
                new List<Ship> {
                    new Ship(
                        ShipType.AircraftCarrier,
                        new List<Position> {
                            new Position(0, 0),
                        }),
                    new Ship(
                        ShipType.Battleship,
                        new List<Position> {
                            new Position(2, 6),
                            new Position(3, 6),
                            new Position(4, 6),
                        }),
                    new Ship(
                        ShipType.Cruiser,
                        new List<Position>
                        {
                            new Position(0, 9),
                            new Position(1, 9),
                        }
                    ),
                    new Ship(
                        ShipType.Destroyer,
                        new List<Position>
                        {
                            new Position(4, 1),
                            new Position(4, 2)
                        }
                    )
                },
                new List<Ship>
                {
                    new Ship(
                        ShipType.Battleship,
                        new List<Position>
                        {
                            new Position(7, 1),
                        }
                    ),
                    new Ship(
                        ShipType.Submarine,
                        new List<Position>
                        {
                            new Position(1, 2),
                            new Position(1, 3),
                        }
                    ),
                    new Ship(
                        ShipType.Destroyer,
                        new List<Position>
                        {
                            new Position(4, 8),
                            new Position(4, 9)
                        }
                    )
                }
            );

            expected.PlayerNameToMove = expected.PlayerOne.PlayerName;

            expected.PlayerOneHits = 2;
            expected.PlayerTwoHits = 1;
            expected.NumberOfTurns = 6;

            expected.PlayerOneGuesses.Add(new Position(1, 1));
            expected.PlayerOneGuesses.Add(new Position(7, 2));
            expected.PlayerOneGuesses.Add(new Position(2, 1));
            expected.PlayerTwoGuesses.Add(new Position(1, 0));
            expected.PlayerTwoGuesses.Add(new Position(5, 5));

            expected.PlayerTwoGuesses.Add(positionToShootAt);

            expected.SinkingAtPreviousHitOfPlayerTwo = false;

            // Act

            underTest.MakeShot(playerToShootWith, positionToShootAt);

            // Assert

            Assert.AreEqual(expected, underTest);

        }

    }
}
