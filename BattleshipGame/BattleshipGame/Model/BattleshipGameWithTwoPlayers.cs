using System;
using System.Collections.Generic;
using System.Text;

namespace BattleshipGame.Model
{
    public class BattleshipGameWithTwoPlayers : BattleshipGame
    {
        public BattleshipGameWithTwoPlayers(Player playerOne, Player playerTwo, List<Ship> playerOneShips, List<Ship> playerTwoShips)
            : base(playerOne, playerTwo, playerOneShips, playerTwoShips)
        {
            PlayerNameToMove = playerOne.PlayerName;
        }
    }
}
