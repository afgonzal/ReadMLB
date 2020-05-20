using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ReadMLB.Entities;

namespace ReadMLB.DataLayer.Mappings
{
    public class PlayerMapping : IEntityTypeConfiguration<Player>
    {
        public void Configure(EntityTypeBuilder<Player> builder)
        {
            builder.HasKey(e => e.PlayerId);
            builder.Property(e => e.PlayerId).ValueGeneratedOnAdd();
            builder.Property(e => e.FirstName).HasMaxLength(50);
            builder.Property(e => e.LastName).HasMaxLength(50);
            builder.Ignore(p => p.PlayerNumerator);
            //builder.HasMany<Batting>(p => p.BattingStats);
            builder.Property(e => e.PrimaryPosition).HasConversion(v => (byte)v, v => (PlayerPositionAbr)v);
            builder.Property(e => e.SecondaryPosition).HasConversion(v => (byte)v, v => (PlayerPositionAbr)v);
            builder.Property(e => e.Bats).HasConversion(v => (byte)v, v => (Bats)v);
            builder.Property(e => e.Throws).HasConversion(v => (byte)v, v => (ThrowHand)v);
            builder.Ignore(e => e.Team);
        }
    }

}
