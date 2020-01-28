using System;
using Domain;
using Microsoft.EntityFrameworkCore;

namespace DAL
{
    public class GameSaveDbContext : DbContext
    {
        public DbSet<GameSave> GameSave { get; set; } = default!;

//        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
//        {
//            base.OnConfiguring(optionsBuilder);
//            optionsBuilder.UseSqlite("Data Source=/Users/fathindosunmu/DataBaseFiles/GameSave.db");
//        }

        public GameSaveDbContext(DbContextOptions options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
    }
}