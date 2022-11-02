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
