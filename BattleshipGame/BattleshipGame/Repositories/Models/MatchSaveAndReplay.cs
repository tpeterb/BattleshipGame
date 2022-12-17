using BattleshipGame.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BattleshipGame.Repositories.Models
{
    public class MatchSaveAndReplay
    {
        [Key]
        public long Id { get; set; }

        [Required]
        public string PlayerName1 { get; set; }
        [Required]
        public string PlayerName2 { get; set; }
        public int NumberOfTurns { get; set; }
        public string Player1Guesses { get; set; }
        public string Player2Guesses { get; set; }
        public string Player1CurrentShips{ get; set; }
        public string Player2CurrentShips { get; set; }
        public string Player1CurrentShipsType { get; set; }
        public string Player2CurrentShipsType { get; set; }
        public string PlayerToStart { get; set; }
        public MatchType Type { get; set; }
    }
}
