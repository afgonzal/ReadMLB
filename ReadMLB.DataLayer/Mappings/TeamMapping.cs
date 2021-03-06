﻿using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ReadMLB.Entities;

namespace ReadMLB.DataLayer.Mappings
{
    public class TeamMapping : IEntityTypeConfiguration<Team>
    {
        public void Configure(EntityTypeBuilder<Team> builder)
        {
            builder.HasKey(e => e.TeamId);
            builder.Property(e => e.TeamAbr).HasMaxLength(3);
            builder.Property(e => e.TeamName).HasMaxLength(50);
            builder.Property(e => e.CityAbr).HasMaxLength(3);
            builder.Property(e => e.CityName).HasMaxLength(50);
            builder.HasOne(e => e.Organization).WithMany(o => o.Affiliates).HasForeignKey(o => o.OrganizationId);
            builder.HasMany<RosterPosition>(e => e.Roster).WithOne(rp => rp.Team).HasForeignKey(rp => rp.TeamId);
        }
    }

}
