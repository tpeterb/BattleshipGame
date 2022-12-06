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

        public Position CreateComputerShot()
        {
            if (!SinkingAtPreviousHitOfPlayerTwo)
            {
                return CreateRandomComputerShot();
            } else
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
            do {
                positionToShootAt = new Position(random.Next(0, BattleshipGame.BoardSize), random.Next(0, BattleshipGame.BoardSize));
            } while (PlayerTwoGuesses.Contains(positionToShootAt));
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
                } else if (direction == Direction.Right) // The square below the previous hit
                {
                    positionToShootAt = lastComputerTarget.GetPositionInDirection(Direction.Right);
                } else if (direction == Direction.Down) // The square to the right of the previous hit
                {
                    positionToShootAt = lastComputerTarget.GetPositionInDirection(Direction.Down);
                } else // The square to the left of the previous hit
                {
                    positionToShootAt = lastComputerTarget.GetPositionInDirection(Direction.Left);
                }
            } while (!IsPositionValid(positionToShootAt)
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
            } else
            {
                PlayerNameToMove = PlayerTwo.PlayerName;
            }
        }
    }
}
