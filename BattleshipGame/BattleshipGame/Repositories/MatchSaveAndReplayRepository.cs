using BattleshipGame.Repositories.Contexts;
using BattleshipGame.Repositories.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BattleshipGame.Repositories
{
    public static class MatchSaveAndReplayRepository
    {
        public static IList<MatchSaveAndReplay> GetMatchSaveAndReplays()
        {
            var database = new DatabaseContext();

            using (database)
            {
                return database.matchSaveAndReplays.ToList();
            }
        }

        public static MatchSaveAndReplay GetMatchId(long id)
        {
            var database = new DatabaseContext();

            using (database)
            {
                return database.matchSaveAndReplays.Where(r => r.Id == id).FirstOrDefault();
            }
        }

        public static void StoreMatchSaveAndReplay(MatchSaveAndReplay match)
        {
            var database = new DatabaseContext();

            using (database)
            {
                database.matchSaveAndReplays.Add(match);

                database.SaveChanges();
            }
        }
    }
}
