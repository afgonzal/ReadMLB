using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ReadMLB.Entities;

namespace ReadMLB.DataLayer.Mappings
{
    public class RotationPositionMapping : IEntityTypeConfiguration<RotationPosition>
    {
        public void Configure(EntityTypeBuilder<RotationPosition> builder)
        {
            builder.HasKey(e => new { e.TeamId, e.League, e.Year, e.InPO, e.Slot });
            builder.ToTable("Rotations");
            builder.HasOne(e => e.Player);
            builder.Property(e => e.PitcherAssignment).HasConversion(v => (byte)v, v => (PitcherAssignment)v);

        }
    }
}