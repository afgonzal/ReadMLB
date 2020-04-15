using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ReadMLB.Entities;
namespace ReadMLB.DataLayer.Mappings
{
    public class BattingMapping : IEntityTypeConfiguration<Batting>
    {
        public void Configure(EntityTypeBuilder<Batting> builder)
        {
            builder.HasKey(e => e.BattingId);
            builder.Property(e => e.BattingId).ValueGeneratedOnAdd();

            builder.Property(e => e.PlayerId).IsRequired();
            builder.Property(e => e.Year).IsRequired();
            builder.Property(e => e.League).IsRequired();
            builder.Property(e => e.InPO).IsRequired();
            builder.ToTable("Batting");
            builder.Property(e => e.H1B).HasColumnName("1B");
            builder.Property(e => e.H2B).HasColumnName("2B");
            builder.Property(e => e.H3B).HasColumnName("3B");
            builder.Property(e => e.PA).HasColumnName("TPA");

        }
    }

}
