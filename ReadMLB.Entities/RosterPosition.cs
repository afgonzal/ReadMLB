using System;
using System.Collections.Generic;
using System.Text;

namespace ReadMLB.Entities
{
    public class RosterPosition
    {
        public byte TeamId { get; set; }
        public byte League { get; set; }
        public short Year { get; set; }

        public bool InPO { get; set; }
        public byte Slot { get; set; }
        public long PlayerId { get; set; }
        public Player Player { get; set; }


    }
}
