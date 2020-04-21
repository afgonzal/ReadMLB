
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using ReadMLB.DataLayer.Mappings;
using ReadMLB.Entities;
using Microsoft.Extensions.Logging.Debug;

namespace ReadMLB.DataLayer.Context
{
    public class FranchiseContext : DbContext
    {
        public static readonly ILoggerFactory Logger = new LoggerFactory(new[] { new DebugLoggerProvider() });
        public FranchiseContext(DbContextOptions<FranchiseContext> options) : base(options)
        {
        }

        public DbSet<Team> Teams { get; set; }

        public DbSet<Player> Players{ get; set; }

        public DbSet<Batting> BattingStats { get; set; }

        public DbSet<Pitching> PitchingStats { get; set; }

        public DbSet<RosterPosition> Rosters { get; set; }

        public DbSet<Running> RunningStats { get; set; }

        public DbSet<Defense> DefenseStats { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new TeamMapping());
            modelBuilder.ApplyConfiguration(new PlayerMapping());
            modelBuilder.ApplyConfiguration(new BattingMapping());
            modelBuilder.ApplyConfiguration(new PitchingMapping());
            modelBuilder.ApplyConfiguration(new RosterPositionMapping());
            modelBuilder.ApplyConfiguration(new RunningMapping());
            modelBuilder.ApplyConfiguration(new DefenseMapping());

        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseLoggerFactory(Logger);
            optionsBuilder.EnableSensitiveDataLogging();
            base.OnConfiguring(optionsBuilder);           
        }
    }
}
