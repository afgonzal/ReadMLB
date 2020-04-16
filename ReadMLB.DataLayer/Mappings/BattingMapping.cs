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
            builder.Property(e => e.BattingVs).HasConversion(v => (byte)v, v => (BattingVs)v);
            builder.Property(e => e.BBK).HasColumnType("decimal(4,3)");
            builder.Property(e => e.BA).HasColumnType("decimal(4,3)");
            builder.Property(e => e.SLG).HasColumnType("decimal(4,3)");
            builder.Property(e => e.OBP).HasColumnType("decimal(4,3)");
            builder.Property(e => e.OPS).HasColumnType("decimal(4,3)");
            builder.Property(e => e.ABHR).HasColumnType("decimal(8,3)");
            builder.HasOne(e => e.Team).WithMany().HasForeignKey(e => e.TeamId);
            builder.HasOne(e => e.Player);
        }
    }

}
