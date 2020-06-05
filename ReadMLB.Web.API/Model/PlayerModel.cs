
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
}
