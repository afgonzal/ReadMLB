using System;
using System.Collections.Generic;
using System.Text;

namespace ReadMLB.Entities
{
    public class Pitching
    {
        public long PitchingId { get; set; }
        public long PlayerId { get; set; }
        public Player Player { get; set; }
        public byte TeamId { get; set; }
        public Team Team { get; set; }
        public byte League { get; set; }
        public bool InPO { get; set; }
        public short Year { get; set; }
        public short G { get; set; }
        public short GS { get; set; }
        public short CG { get; set; }
        public short SHO { get; set; }
        public short W { get; set; }
        public short L { get; set; }

        public short SV { get; set; }

        public short BSV { get; set; }

        public short R { get; set; }
        public short ER { get; set; }

        public float IP { get; set; }
        public short K { get; set; }
        public short BB { get; set; }
        public short H { get; set; }
        public short? PK { get; set; }
        public short? TPA { get; set; }
        public short? H1B { get; set; }
        public short? H2B { get; set; }
        public short? H3B { get; set; }
        public short HR { get; set; }
        public short? IBB { get; set; }
        public short? HB { get; set; }
        public short? SF { get; set; }
        public short? SH { get; set; }
    }
}
