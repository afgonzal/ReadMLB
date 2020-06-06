
namespace ReadMLB.Web.API.Model
{
    public class BattingStatModel
    {
        public short Year { get; set; }
        public byte League { get; set; }
        public byte TeamId { get; set; }
        public string TeamAbr { get; set; }
        public string TeamName { get; set; }

        public string Organization { get; set; }
        
        public bool InPO { get; set; }
        public string BattingVs { get; set; }
        public short G { get; set; }
        public short PA { get; set; }
        public short H1B { get; set; }
        public short H2B { get; set; }
        public short H3B { get; set; }
        public short HR { get; set; }
        public short RBI { get; set; }
        public short K { get; set; }
        public short BB { get; set; }
        public short SH { get; set; }

        public short SF { get; set; }

        public short HBP { get; set; }
        public short IBB { get; set; }

        public short H { get; set; }

        public short TB { get; set; }


        public short AB { get; set; }

        public short XBH { get; set; }

        public float BBK { get; set; }


        public float BA { get; set; }


        public float SLG { get; set; }

        public float ABHR { get; set; }


        public float OBP { get; set; }


        public float OPS { get; set; }
    }
    public class BattingAndPlayerStatModel : BattingStatModel
    {
        public string PlayerName { get; set; }
        public long PlayerId { get; set; }

    }
}
