using System;
using System.Collections.Generic;

namespace ReadMLB.Entities
{
    public class Team
    {
        public byte TeamId { get; set; }
        public long? TeamNum { get; set; }
        public string TeamAbr { get; set; }
        public string TeamName { get; set; }
        public string CityAbr { get; set; }
        public string CityName { get; set; }
        public byte? OrganizationId { get; set; }
        public byte? League { get; set; }
        public byte? Division { get; set; }

        public Team Organization { get; set; }

        public ICollection<Team> Affiliates { get; set; }
        public ICollection<RosterPosition> Roster { get; set; }
    }
}
