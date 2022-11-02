using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BattleshipGame.Model
{
    public abstract class BattleshipGame
    {
        public const int BoardSize = 10;

        public int PlayerOneHits { get; set; }

        public int PlayerTwoHits { get; set; }

        public int NumberOfTurns { get; set; }

        public Player PlayerOne { get; set; }

        public Player PlayerTwo { get; set; }

        public List<Ship> PlayerOneOriginalShips { get; set; }

        public List<Ship> PlayerOneCurrentShips { get; set; }

        public List<Position> PlayerOneGuesses { get; set; }

        public List<Ship> PlayerTwoOriginalShips { get; set; }

        public List<Ship> PlayerTwoCurrentShips { get; set; }

        public List<Position> PlayerTwoGuesses { get; set; }

        public string PlayerNameToMove { get; protected set; }

        public bool SinkingAtPreviousHitOfPlayerOne { get; protected set; }

        public bool SinkingAtPreviousHitOfPlayerTwo { get; protected set; }

        protected BattleshipGame()
        {
            PlayerOneHits = 0;
            PlayerTwoHits = 0;
            NumberOfTurns = 0;
            PlayerOneGuesses = new List<Position>();
            PlayerTwoGuesses = new List<Position>();
        }

        protected BattleshipGame(Player playerOne, Player playerTwo, List<Ship> playerOneShips, List<Ship> playerTwoShips)
            : this()
        {
            PlayerOne = playerOne;
            PlayerTwo = playerTwo;
            PlayerOneOriginalShips = playerOneShips;
            PlayerTwoOriginalShips = playerTwoShips;
            PlayerOneCurrentShips = playerOneShips.Select(ship => new Ship(ship)).ToList();
            PlayerTwoCurrentShips = playerTwoShips.Select(ship => new Ship(ship)).ToList();
        }

        public void MakeShot(Player playerToShoot, Position positionToShootAt)
        {
            if (!IsPositionValid(positionToShootAt))
            {
                throw new ArgumentException("Shooting at a position that's not present on the board!");
            }
            if (CanMakeShot(playerToShoot, positionToShootAt))
            {
                ProcessShot(playerToShoot, positionToShootAt);
                ChangePlayerNameToMove();
            }
        }

        protected void ProcessShot(Player playerToShoot, Position positionToShootAt)
        {
            if (playerToShoot.PlayerName == PlayerOne.PlayerName)
            {
                PlayerOneGuesses.Add(positionToShootAt);
                if (ShotHitsAShip(PlayerTwoCurrentShips, positionToShootAt))
                {
                    PlayerOneHits++;
                    SinkingAtPreviousHitOfPlayerOne = true;
                    DestroyShipPart(PlayerTwoCurrentShips, positionToShootAt);
                } else
                {
                    SinkingAtPreviousHitOfPlayerOne = false;
                }
            } else if (playerToShoot.PlayerName == PlayerTwo.PlayerName)
            {
                PlayerTwoGuesses.Add(positionToShootAt);
                if (ShotHitsAShip(PlayerOneCurrentShips, positionToShootAt))
                {
                    PlayerTwoHits++;
                    SinkingAtPreviousHitOfPlayerTwo = true;
                    DestroyShipPart(PlayerOneCurrentShips, positionToShootAt);
                } else
                {
                    SinkingAtPreviousHitOfPlayerTwo = false;
                }
            }
            NumberOfTurns++;
        }

        protected bool CanMakeShot(Player playerToShoot, Position positionToShootAt)
        {
            if (playerToShoot.PlayerName != PlayerNameToMove)
            {
                return false;
            }
            if (playerToShoot.PlayerName == PlayerOne.PlayerName)
            {
                if (PlayerOneGuesses.Contains(positionToShootAt))
                {
                    return false;
                }
                return true;
            } else if (playerToShoot.PlayerName == PlayerTwo.PlayerName)
            {
                if (PlayerTwoGuesses.Contains(positionToShootAt))
                {
                    return false;
                }
                return true;
            }
            return false;
        }

        protected bool ShotHitsAShip(List<Ship> shipsToCheckForHit, Position positionToShootAt)
        {
            foreach (var ship in shipsToCheckForHit)
            {
                foreach (var position in ship.ShipPositions)
                {
                    if (position == positionToShootAt)
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        protected void DestroyShipPart(List<Ship> shipsAmongWhichToDestroyAShipPart, Position shipPartToDestroy)
        {
            foreach (var ship in shipsAmongWhichToDestroyAShipPart)
            {
                for (int i = 0; i < ship.ShipPositions.Count; i++)
                {
                    if (ship.ShipPositions.ElementAt(i) == shipPartToDestroy)
                    {
                        ship.ShipPositions.RemoveAt(i);
                        if (ship.GetCurrentShipSize() == 0)
                        {
                            ship.Destroyed = true;
                        }
                        return;
                    }
                }
            }
        }

        protected void ChangePlayerNameToMove()
        {
            if (PlayerNameToMove == PlayerOne.PlayerName)
            {
                PlayerNameToMove = PlayerTwo.PlayerName;
            } else
            {
                PlayerNameToMove = PlayerOne.PlayerName;
            }
        }

        public static bool IsPositionValid(Position position)
        {
            return position.Row >= 0
                && position.Column >= 0
                && position.Row < BoardSize
                && position.Column < BoardSize;
        }

    }
}
