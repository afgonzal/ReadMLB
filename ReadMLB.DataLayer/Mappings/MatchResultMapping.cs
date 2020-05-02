using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ReadMLB.Entities;

namespace ReadMLB.DataLayer.Mappings
{
    public class MatchResultMapping : IEntityTypeConfiguration<MatchResult>
    {
        public void Configure(EntityTypeBuilder<MatchResult> builder)
        {
            builder.HasKey(e => e.MatchId);
            builder.Property(e => e.MatchId).ValueGeneratedOnAdd();
            builder.ToTable("Schedule");

            builder.Property(e => e.HomeTeamId).IsRequired();
            builder.Property(e => e.AwayTeamId).IsRequired();
        }
    }
}