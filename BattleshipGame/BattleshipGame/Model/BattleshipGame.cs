using System;
using System.Collections.Generic;
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


        private List<Ship> _playerOneShips;

        private List<Position> _playerOneGuesses;

        private List<Ship> _playerTwoShips;

        private List<Position> _playerTwoGuesses;

        public string playerNameToMove { get; set; }

        protected BattleshipGame(Player playerOne, Player playerTwo, List<Ship> playerOneShips, List<Ship> playerTwoShips)
        {
            PlayerOne = playerOne;
            PlayerTwo = playerTwo;
            _playerOneShips = playerOneShips;
            _playerTwoShips = playerTwoShips;
            PlayerOneHits = 0;
            PlayerTwoHits = 0;
            NumberOfTurns = 0;
            _playerOneGuesses = new List<Position>();
            _playerTwoGuesses = new List<Position>();
        }

    }
}
