using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BattleshipGame.Model
{
    public class BattleshipGameAgainstComputer : BattleshipGame
    {
        public BattleshipGameAgainstComputer(Player humanPlayer, List<Ship> playerOneShips, List<Ship> playerTwoShips)
            : base()
        {
            PlayerOne = humanPlayer;
            PlayerTwo = new Player(PlayerType.Computer, "AI");
            PlayerOneOriginalShips = playerOneShips;
            PlayerTwoOriginalShips = playerTwoShips;
            PlayerOneCurrentShips = playerOneShips.Select(ship => new Ship(ship)).ToList();
            PlayerTwoCurrentShips = playerTwoShips.Select(ship => new Ship(ship)).ToList();
            DetermineStartingPlayer();
        }

        public BattleshipGameAgainstComputer(Player humanPlayer, List<Ship> playerOneShips)
        {
            PlayerOne = humanPlayer;
            PlayerTwo = new Player(PlayerType.Computer, "AI");
            PlayerOneOriginalShips = playerOneShips;
            List<Ship> playerTwoShips = CreateStartingPositionForComputer();
            PlayerTwoOriginalShips = playerTwoShips;
            PlayerOneCurrentShips = playerOneShips.Select(ship => new Ship(ship)).ToList();
            PlayerTwoCurrentShips = playerTwoShips.Select(ship => new Ship(ship)).ToList();
            DetermineStartingPlayer();
        }

        public static List<Ship> CreateStartingPositionForComputer()
        {
            List<Ship> computerShips = new List<Ship>();
            int shipLength = 0;
            Random random = new Random();
            ShipOrientation orientation;
            foreach (var ship in Enum.GetValues(typeof(ShipType)))
            {
                orientation = (ShipOrientation)random.Next(0, 2);
                switch (ship)
                {
                    case ShipType.AircraftCarrier:
                        shipLength = Ship.AircraftCarrierSize;
                        computerShips.Add(new Ship(
                            ShipType.AircraftCarrier,
                            CreatePositionsForShip(orientation, shipLength, computerShips)));
                        break;
                    case ShipType.Battleship:
                        shipLength = Ship.BattleshipSize;
                        computerShips.Add(new Ship(
                            ShipType.Battleship,
                            CreatePositionsForShip(orientation, shipLength, computerShips)));
                        break;
                    case ShipType.Cruiser:
                        shipLength = Ship.CruiserSize;
                        computerShips.Add(new Ship(
                            ShipType.Cruiser,
                            CreatePositionsForShip(orientation, shipLength, computerShips)));
                        break;
                    case ShipType.Submarine:
                        shipLength = Ship.SubmarineSize;
                        computerShips.Add(new Ship(
                            ShipType.Submarine,
                            CreatePositionsForShip(orientation, shipLength, computerShips)));
                        break;
                    case ShipType.Destroyer:
                        shipLength = Ship.DestroyerSize;
                        computerShips.Add(new Ship(
                            ShipType.Destroyer,
                            CreatePositionsForShip(orientation, shipLength, computerShips)));
                        break;
                }
            }
            return computerShips;
        }

        private static List<Position> CreatePositionsForShip(ShipOrientation orientation, int shipLength, List<Ship> computerShips)
        {
            List<Position> shipPositions = new List<Position>();
            Random random = new Random();
            Position startingPosition;
            do
            {
                startingPosition = new Position(random.Next(BoardSize), random.Next(BoardSize));
            }
            while (!DoesShipFitOnBoard(orientation, shipLength, startingPosition, computerShips, out shipPositions));
            return shipPositions;
        }

        private static bool DoesShipFitOnBoard(ShipOrientation orientation, int shipLength, Position startingPosition, List<Ship> computerShips, out List<Position> shipPositions)
        {
            List<Position> positions = new List<Position>();
            Position tempPosition = new Position(startingPosition);
            Random random = new Random();
            switch (orientation)
            {
                case ShipOrientation.Vertical:
                    // Checking if the table is wide enough in either direction for the ship starting at the randomly generated position
                    if (!IsPositionValid(new Position(startingPosition.Row - shipLength + 1, startingPosition.Column))
                        && !IsPositionValid(new Position(startingPosition.Row + shipLength - 1, startingPosition.Column)))
                    {
                        shipPositions = new List<Position>();
                        return false;
                    }
                    List<Direction> possibleVerticalDirections = new List<Direction>
                    {
                        Direction.Down,
                        Direction.Up
                    };
                    int verticalDirectionIndex = random.Next(0, 2);
                    Direction verticalDirection = possibleVerticalDirections[verticalDirectionIndex];
                    List<Direction> verticalDirectionsAlreadyTried = new List<Direction>();
                    verticalDirectionsAlreadyTried.Add(verticalDirection);
                    bool changedVerticalDirection = false;
                    while (positions.Count != shipLength)
                    {
                        if (!IsPositionValid(tempPosition))
                        {
                            if (verticalDirectionsAlreadyTried.Count == possibleVerticalDirections.Count)
                            {
                                shipPositions = new List<Position>();
                                return false;
                            }
                            verticalDirectionIndex = 1 - verticalDirectionIndex;
                            verticalDirection = possibleVerticalDirections[verticalDirectionIndex];
                            verticalDirectionsAlreadyTried.Add(verticalDirection);
                            positions.Clear();
                            tempPosition = startingPosition;
                            continue;
                        }
                        // Checking if the generated ship positions collide with any already existing computer ship
                        foreach (var ship in computerShips)
                        {
                            foreach (var position in ship.ShipPositions)
                            {
                                if (position.Equals(tempPosition))
                                {
                                    if (verticalDirectionsAlreadyTried.Count == possibleVerticalDirections.Count)
                                    {
                                        shipPositions = new List<Position>();
                                        return false;
                                    }
                                    verticalDirectionIndex = 1 - verticalDirectionIndex;
                                    verticalDirection = possibleVerticalDirections[verticalDirectionIndex];
                                    verticalDirectionsAlreadyTried.Add(verticalDirection);
                                    positions.Clear();
                                    tempPosition = startingPosition;
                                    changedVerticalDirection = true;
                                    break;
                                }
                            }
                        }
                        // There has just been a change in direction to investigate
                        if (changedVerticalDirection)
                        {
                            changedVerticalDirection = false;
                            continue;
                        }
                        positions.Add(tempPosition);
                        tempPosition = tempPosition.GetPositionInDirection(verticalDirection);
                    }
                    shipPositions = positions;
                    return true;
                case ShipOrientation.Horizontal:
                    // Checking if the table is wide enough in either direction for the ship starting at the randomly generated position
                    if (!IsPositionValid(new Position(startingPosition.Row, startingPosition.Column - shipLength + 1))
                        && !IsPositionValid(new Position(startingPosition.Row, startingPosition.Column + shipLength - 1)))
                    {
                        shipPositions = new List<Position>();
                        return false;
                    }
                    List<Direction> possibleHorizontalDirections = new List<Direction>
                    {
                        Direction.Right,
                        Direction.Left
                    };
                    int horizontalDirectionIndex = random.Next(0, 2);
                    Direction horizontalDirection = possibleHorizontalDirections[horizontalDirectionIndex];
                    List<Direction> horizontalDirectionsAlreadyTried = new List<Direction>();
                    horizontalDirectionsAlreadyTried.Add(horizontalDirection);
                    bool changedHorizontalDirection = false;
                    while (positions.Count != shipLength)
                    {
                        if (!IsPositionValid(tempPosition))
                        {
                            if (horizontalDirectionsAlreadyTried.Count == possibleHorizontalDirections.Count)
                            {
                                shipPositions = new List<Position>();
                                return false;
                            }
                            horizontalDirectionIndex = 1 - horizontalDirectionIndex;
                            horizontalDirection = possibleHorizontalDirections[horizontalDirectionIndex];
                            horizontalDirectionsAlreadyTried.Add(horizontalDirection);
                            positions.Clear();
                            tempPosition = startingPosition;
                            continue;
                        }
                        // Checking if the generated ship positions collide with any already existing computer ship
                        foreach (var ship in computerShips)
                        {
                            foreach (var position in ship.ShipPositions)
                            {
                                if (position.Equals(tempPosition))
                                {
                                    if (horizontalDirectionsAlreadyTried.Count == possibleHorizontalDirections.Count)
                                    {
                                        shipPositions = new List<Position>();
                                        return false;
                                    }
                                    horizontalDirectionIndex = 1 - horizontalDirectionIndex;
                                    horizontalDirection = possibleHorizontalDirections[horizontalDirectionIndex];
                                    horizontalDirectionsAlreadyTried.Add(horizontalDirection);
                                    positions.Clear();
                                    tempPosition = startingPosition;
                                    changedHorizontalDirection = true;
                                    break;
                                }
                            }
                        }
                        // There has just been a change in direction to investigate
                        if (changedHorizontalDirection)
                        {
                            changedHorizontalDirection = false;
                            continue;
                        }
                        positions.Add(tempPosition);
                        tempPosition = tempPosition.GetPositionInDirection(horizontalDirection);
                    }
                    shipPositions = positions;
                    return true;
            }
            shipPositions = new List<Position>();
            return false;
        }

        public Position CreateComputerShot()
        {
            if (!SinkingAtPreviousHitOfPlayerTwo)
            {
                return CreateRandomComputerShot();
            }
            else
            {
                Position lastComputerTarget = PlayerTwoGuesses.Last();
                Position positionToTheLeftOfPreviousTarget = lastComputerTarget.GetPositionInDirection(Direction.Left);
                Position positionToTheRightOfPreviousTarget = lastComputerTarget.GetPositionInDirection(Direction.Right);
                Position positionAboveThePreviousTarget = lastComputerTarget.GetPositionInDirection(Direction.Up);
                Position positionBelowThePreviousTarget = lastComputerTarget.GetPositionInDirection(Direction.Down);
                if (PlayerTwoGuesses.Contains(positionToTheLeftOfPreviousTarget)
                && PlayerTwoGuesses.Contains(positionToTheRightOfPreviousTarget)
                && PlayerTwoGuesses.Contains(positionAboveThePreviousTarget)
                && PlayerTwoGuesses.Contains(positionBelowThePreviousTarget))
                {
                    return CreateRandomComputerShot();
                }
                return CreateComputerShotNextToPreviousHit();
            }
        }

        public List<Ship> GetComputerShips()
        {
            return PlayerTwoOriginalShips;
        }

        private Position CreateRandomComputerShot()
        {
            Position positionToShootAt;
            Random random = new Random();
            do
            {
                positionToShootAt = new Position(random.Next(0, BattleshipGame.BoardSize), random.Next(0, BattleshipGame.BoardSize));
            }
            while (PlayerTwoGuesses.Contains(positionToShootAt));
            return positionToShootAt;
        }

        private Position CreateComputerShotNextToPreviousHit()
        {
            Position positionToShootAt;
            Position lastComputerTarget = PlayerTwoGuesses.Last();
            Random random = new Random();
            do
            {
                Direction direction = (Direction)random.Next(0, 4);
                // The square above the previous hit
                if (direction == Direction.Up)
                {
                    positionToShootAt = lastComputerTarget.GetPositionInDirection(Direction.Up);
                }
                else if (direction == Direction.Right) // The square below the previous hit
                {
                    positionToShootAt = lastComputerTarget.GetPositionInDirection(Direction.Right);
                }
                else if (direction == Direction.Down) // The square to the right of the previous hit
                {
                    positionToShootAt = lastComputerTarget.GetPositionInDirection(Direction.Down);
                }
                else // The square to the left of the previous hit
                {
                    positionToShootAt = lastComputerTarget.GetPositionInDirection(Direction.Left);
                }
            }
            while (!IsPositionValid(positionToShootAt)
                     || PlayerTwoGuesses.Contains(positionToShootAt));
            return positionToShootAt;
        }

        private void DetermineStartingPlayer()
        {
            Random random = new Random();
            int randomResult = random.Next(1, 3);
            if (randomResult == 1)
            {
                PlayerNameToMove = PlayerOne.PlayerName;
            }
            else
            {
                PlayerNameToMove = PlayerTwo.PlayerName;
            }
            PlayerToStart = PlayerNameToMove;
        }
    }
}
