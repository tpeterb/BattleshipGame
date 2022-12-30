using System.Collections.Generic;
using System.Linq;
using BattleshipGame.Repositories.Contexts;
using BattleshipGame.Repositories.Models;

namespace BattleshipGame.Repositories
{
    public static class MatchScoreRepository
    {
        public static IList<MatchScore> GetMatchScores()
        {
            var database = new DatabaseContext();

            using (database)
            {
                return database.MatchScores.ToList();
            }
        }

        public static void StoreMatchScore(MatchScore match)
        {
            var database = new DatabaseContext();

            using (database)
            {
                database.MatchScores.Add(match);

                database.SaveChanges();
            }
        }
    }
}
