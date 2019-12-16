using Domain;
using Microsoft.EntityFrameworkCore;

namespace DAL
{
    public class AppDbContext: DbContext
    {
        public DbSet<GameSettings> GameSettings { get; set; }  = default!;
        public DbSet<GameState> GameStates { get; set; } = default!;
        public DbSet<Move> Move { get; set; } = default!;


        public AppDbContext(DbContextOptions options) : base(options)
        {
            
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
        

    }
}