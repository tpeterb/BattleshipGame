using System.Collections.Generic;
using System.Linq;
using BattleshipGame.Repositories.Contexts;
using BattleshipGame.Repositories.Models;

namespace BattleshipGame.Repositories
{
    public static class MatchSaveAndReplayRepository
    {
        public static IList<MatchSaveAndReplay> GetMatchSaveAndReplays()
        {
            var database = new DatabaseContext();

            using (database)
            {
                return database.MatchSaveAndReplays.ToList();
            }
        }

        public static MatchSaveAndReplay GetMatchId(long id)
        {
            var database = new DatabaseContext();

            using (database)
            {
                return database.MatchSaveAndReplays.Where(r => r.Id == id).FirstOrDefault();
            }
        }

        public static void StoreMatchSaveAndReplay(MatchSaveAndReplay match)
        {
            var database = new DatabaseContext();

            using (database)
            {
                database.MatchSaveAndReplays.Add(match);

                database.SaveChanges();
            }
        }
    }
}
