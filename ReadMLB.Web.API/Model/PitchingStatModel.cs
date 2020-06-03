using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace ReadMLB.Web.API.Model
{
    public class PitchingStatModel
    {
        public long PitchingId { get; set; }
        public long PlayerId { get; set; }
        public byte TeamId { get; set; }
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
        public short IP10 { get; set; }
        public short K { get; set; }
        public short BB { get; set; }
        /// <summary>
        /// these one comes from the DB, useful for minor leagues that don't record 1B, 2B etc, no need to calculate it
        /// </summary>
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


        public float WP { get; set; }

        public float BB9 { get; set; }
        public float H9 { get; set; }

        public float HR9 { get; set; }
        public float K9 { get; set; }

        public float WHIP { get; set; }
        public short IPG { get; set; }
        public float ERA { get; set; }

        public float KBB { get; set; }

        public string TeamAbr { get; set; }
        public string TeamName { get; set; }

        public string Organization { get; set; }

    }
}
