
namespace ReadMLB.Entities
{
    public class Alignment
    {
        public long AlignmentId { get; set; }

        public byte TeamId { get; set; }
        
        public short Year { get; set; }

        public byte League { get; set; }

        public bool InPO { get; set; }

        public long PlayerId{ get; set; }

        public AlignmentVs AlignmentVs { get; set; }
        public PlayerPosition Position{ get; set; }
    }
}
