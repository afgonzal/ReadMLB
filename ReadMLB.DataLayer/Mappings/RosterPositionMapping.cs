using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ReadMLB.Entities;

namespace ReadMLB.DataLayer.Mappings
{
    public class RosterPositionMapping : IEntityTypeConfiguration<RosterPosition>
    {
        public void Configure(EntityTypeBuilder<RosterPosition> builder)
        {
            builder.HasKey(e => new {e.TeamId, e.League, e.Year,e.InPO, e.Slot} );
            builder.ToTable("Rosters");
            builder.HasOne(e => e.Player);

        }
    }
}