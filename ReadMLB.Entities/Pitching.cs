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


        public float WP
        {
            get => (W + L > 0) ? (float) Math.Round((float)(W*100) / (float)(W + L), 3) : 0;
            protected set{}
        }

        public float BB9
        {
            get => (float) (IP10 > 0 ? BB *90 / IP10 : 0);
            protected set {}
        }
        public float H9
        {
            get => (float)(IP10 > 0 ? H *90 / IP10  : 0);
            protected set { }
        }

        public float HR9
        {
            get => (float)(IP10 > 0 ? HR *90 / IP10 : 0);
            protected set { }
        }
        public float K9
        {
            get => (float)(IP10 > 0 ? K *90 / IP10 : 0);
            protected set { }
        }

        public float WHIP
        {
            get => (float) (IP10 > 0 ? (H + BB) / IP10 / 10 : 0);
            protected set{}
        }
        //TODO mal calculada
        /// <summary>
        /// Est'a mal
        /// </summary>
        public short IPG
        {
            get => (short) (G > 0 ? IP10 / G / 10 : 0);
            protected set {}
        }

    }
}
