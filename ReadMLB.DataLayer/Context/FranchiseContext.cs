
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using ReadMLB.DataLayer.Mappings;
using ReadMLB.Entities;
using Microsoft.Extensions.Logging.Debug;

namespace ReadMLB.DataLayer.Context
{
    public class FranchiseContext : DbContext
    {
        public static readonly ILoggerFactory _logger = new LoggerFactory(new[] { new DebugLoggerProvider() });
        public FranchiseContext(DbContextOptions<FranchiseContext> options) : base(options)
        {
        }

        public DbSet<Team> Teams { get; set; }

        public DbSet<Player> Players{ get; set; }

        public DbSet<Batting> BattingStats { get; set; }

        public DbSet<Pitching> PitchingStats { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new TeamMapping());
            modelBuilder.ApplyConfiguration(new PlayerMapping());
            modelBuilder.ApplyConfiguration(new BattingMapping());
            modelBuilder.ApplyConfiguration(new PitchingMapping());

        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseLoggerFactory(_logger);
            optionsBuilder.EnableSensitiveDataLogging();
            base.OnConfiguring(optionsBuilder);           
        }
    }
}
