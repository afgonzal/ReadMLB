using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ReadMLB.Entities;

namespace ReadMLB.DataLayer.Mappings
{
    public class DefenseMapping : IEntityTypeConfiguration<Defense>
    {
        public void Configure(EntityTypeBuilder<Defense> builder)
        {
            builder.HasKey(e => e.DefenseId);
            builder.Property(e => e.DefenseId).ValueGeneratedOnAdd();
            builder.ToTable("Defense");
            builder.Property(e => e.Fld).HasColumnType("decimal(4,3)");

        }
    }
}