using System;
using System.Collections.Generic;
using System.Text;

namespace ReadMLB.Entities
{
    public class Running
    {
        public long RunningId { get; set; }
        public long PlayerId { get; set; }
        public Player Player { get; set; }
        public short Year { get; set; }
        public byte TeamId { get; set; }
        public byte League { get; set; }
        public bool InPO { get; set; }
        public short RS { get; set; }
        public short SB { get; set; }
        public short CS { get; set; }
    }
}
