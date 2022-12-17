using BattleshipGame.Repositories.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Text;

namespace BattleshipGame.Repositories.Contexts
{
    public class DatabaseContext : DbContext
    {
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Server=(localdb)\\mssqllocaldb;Database=BattleshipGameDB;Integrated Security=True;");
        }

        public DbSet<MatchSaveAndReplay> matchSaveAndReplays { get; set; }
        public DbSet<MatchScore> matchScores { get; set; }
    }
}
