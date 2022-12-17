using BattleshipGame.Model;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BattleshipGame.Repositories.Models
{
    public class MatchScore
    {
        [Key] 
        public long Id { get; set; }
        [Required]
        public string PlayerName1 { get; set; }
        [Required]
        public string PlayerName2 { get; set; }
        public int NumberOfTurns { get; set; }
        public string Player1Hits { get; set; }
        public string Player2Hits { get; set; }
        public string WinnerPlayerName { get; set; }
    }
}
