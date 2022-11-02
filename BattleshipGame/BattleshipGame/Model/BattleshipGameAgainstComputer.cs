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
                int randomResult = random.Next(1, 5);
                // The square above the previous hit
                if (randomResult == 1)
                {
                    positionToShootAt = new Position(lastComputerTarget.Row - 1, lastComputerTarget.Column);
                } else if (randomResult == 2) // The square below the previous hit
                {
                    positionToShootAt = new Position(lastComputerTarget.Row + 1, lastComputerTarget.Column);
                } else if (randomResult == 3) // The square to the right of the previous hit
                {
                    positionToShootAt = new Position(lastComputerTarget.Row, lastComputerTarget.Column + 1);
                } else // The square to the left of the previous hit
                {
                    positionToShootAt = new Position(lastComputerTarget.Row, lastComputerTarget.Column - 1);
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
