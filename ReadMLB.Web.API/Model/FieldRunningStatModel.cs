namespace ReadMLB.Web.API.Model
{
    public class FieldRunningStatModel
    {
        public byte TeamId { get; set; }
        public short Year { get; set; }
        public byte League { get; set; }
        public bool InPO { get; set; }
        public string TeamAbr { get; set; }
        public string TeamName { get; set; }

        public string Organization { get; set; }

        public long DefenseId { get; set; }
        public short ASST { get; set; }
        public short ERR { get; set; }
        public short PO { get; set; }

        public float Fld { get; set; }

        public short RS { get; set; }
        public short SB { get; set; }
        public short CS { get; set; }
        public short Ch { get; set; }
    }
}