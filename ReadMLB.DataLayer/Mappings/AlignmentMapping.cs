using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ReadMLB.Entities;

namespace ReadMLB.DataLayer.Mappings
{
    public class AlignmentMapping : IEntityTypeConfiguration<Alignment>
    {
        public void Configure(EntityTypeBuilder<Alignment> builder)
        {
            builder.HasKey(e => e.AlignmentId);
            builder.Property(e => e.AlignmentId).ValueGeneratedOnAdd();
            builder.ToTable("Batting");

            builder.Property(e => e.PlayerId).IsRequired();
            builder.Property(e => e.Year).IsRequired();
            builder.Property(e => e.League).IsRequired();
            builder.Property(e => e.InPO).IsRequired();
            builder.Property(e => e.AlignmentVs).HasConversion(v => (byte)v, v => (AlignmentVs)v);
        }
    }
}