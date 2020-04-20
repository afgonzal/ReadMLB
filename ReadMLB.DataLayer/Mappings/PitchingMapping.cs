using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ReadMLB.Entities;
namespace ReadMLB.DataLayer.Mappings
{
    public class PitchingMapping : IEntityTypeConfiguration<Pitching>
    {
        public void Configure(EntityTypeBuilder<Pitching> builder)
        {
            builder.HasKey(e => e.PitchingId);
            builder.Property(e => e.PitchingId).ValueGeneratedOnAdd();
            builder.ToTable("Pitching");


            builder.Property(e => e.PlayerId).IsRequired();
            builder.Property(e => e.Year).IsRequired();
            builder.Property(e => e.League).IsRequired();
            builder.Property(e => e.InPO).IsRequired();
            builder.Property(e => e.H1B).HasColumnName("1B");
            builder.Property(e => e.H2B).HasColumnName("2B");
            builder.Property(e => e.H3B).HasColumnName("3B");
            builder.HasOne(e => e.Team).WithMany().HasForeignKey(e => e.TeamId);
            builder.HasOne(e => e.Player);
            
            builder.Property(e => e.WP).HasColumnType("decimal(6,3)");
            builder.Property(e => e.H9).HasColumnType("decimal(6,3)");
            builder.Property(e => e.HR9).HasColumnType("decimal(6,3)");
            builder.Property(e => e.BB9).HasColumnType("decimal(6,3)");
            builder.Property(e => e.H9).HasColumnType("decimal(6,3)");
            builder.Property(e => e.H9).HasColumnType("decimal(6,3)");
            builder.Property(e => e.ERA).HasColumnType("decimal(6,3)");
            builder.Property(e => e.KBB).HasColumnType("decimal(6,3)");

        }
    }

}
