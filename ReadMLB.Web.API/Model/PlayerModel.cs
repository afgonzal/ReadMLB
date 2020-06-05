
using System.Collections.Generic;

namespace ReadMLB.Web.API.Model
{
    public class PlayerModel
    {
        public long PlayerId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public byte? Shirt { get; set; }
        public string PrimaryPosition { get; set; }
        public string SecondaryPosition { get; set; }

        public string Bats { get; set; }
        public string Throws { get; set; }
        public TeamModel Team { get; set; }
    }

    public class PlayerTeamHistoryModel
    {
        public byte TeamId { get; set; }
        public string TeamAbr { get; set; }
        public string TeamName { get; set; }
        public byte OrganizationId { get; set; }
        public string Organization { get; set; }
        public byte League { get; set; }
        public short Year { get; set; }

        public bool InPO { get; set; }
        public byte Slot { get; set; }
    }
    public class PlayerWithHistoryModel : PlayerModel
    {
        public IEnumerable<PlayerTeamHistoryModel> TeamHistory { get; set; }
    }
}
