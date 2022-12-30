using BattleshipGame.Repositories.Models;
using Microsoft.EntityFrameworkCore;

namespace BattleshipGame.Repositories.Contexts
{
    public class DatabaseContext : DbContext
    {
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Server=(localdb)\\mssqllocaldb;Database=BattleshipGameDB;Integrated Security=True;");
        }

        public DbSet<MatchSaveAndReplay> MatchSaveAndReplays { get; set; }
        public DbSet<MatchScore> MatchScores { get; set; }
    }
}
