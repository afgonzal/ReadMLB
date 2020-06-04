using System.Collections.Generic;

namespace ReadMLB.Web.API.Model
{
    public class TeamModel
    {
        public byte TeamId { get; set; }
        public byte League { get; set; }
        public byte Division { get; set; }
        public string TeamName { get; set; }
        public string TeamAbr { get; set; }

        public string CityName { get; set; }
        public byte OrganizationId { get; set; }
        public string Organization { get; set; }
    }

    public class DivisionModel
    {
        public byte League { get; set; }
        public byte DivisionId { get; set; }
        public string Name { get; set; }
        public ICollection<TeamModel> Teams { get; set; }
    }
}