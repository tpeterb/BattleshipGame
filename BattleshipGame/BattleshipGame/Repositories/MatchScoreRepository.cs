using BattleshipGame.Repositories.Contexts;
using BattleshipGame.Repositories.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BattleshipGame.Repositories
{
    public class MatchScoreRepository
    {

        public static IList<MatchScore> GetMatchScores()
        {
            var database = new DatabaseContext();

            using (database)
            {
                return database.matchScores.ToList();
            }
        }

        public static void StoreMatchScore(MatchScore match)
        {
            var database = new DatabaseContext();

            using (database)
            {
                database.matchScores.Add(match);

                database.SaveChanges();
            }
        }
    }
}
