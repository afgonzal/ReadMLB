
namespace ReadMLB.Web.API.Model
{
    public class RunningStatModel
    {
        public long RunningId { get; set; }
        public short Year { get; set; }
        public byte TeamId { get; set; }

        public string TeamAbr { get; set; }
        public string TeamName { get; set; }

        public string Organization { get; set; }
        public byte League { get; set; }
        public bool InPO { get; set; }
        public short RS { get; set; }
        public short SB { get; set; }
        public short CS { get; set; }
    }
}
