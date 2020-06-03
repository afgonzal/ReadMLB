using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ReadMLB.Entities;

namespace ReadMLB.DataLayer.Mappings
{
    public class RunningMapping : IEntityTypeConfiguration<Running>
    {
        public void Configure(EntityTypeBuilder<Running> builder)
        {
            builder.HasKey(e => e.RunningId);
            builder.Property(e => e.RunningId).ValueGeneratedOnAdd();
            builder.HasOne(e => e.Team).WithMany().HasForeignKey(e => e.TeamId);
            builder.ToTable("Running");
        }
    }
}