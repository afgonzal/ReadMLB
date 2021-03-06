﻿using System;
using System.Collections.Generic;
using System.Text;

namespace ReadMLB.Entities
{
    public class Defense
    {
        public long DefenseId { get; set; }
        public long PlayerId { get; set; }
        public byte TeamId { get; set; }
        public short Year { get; set; }
        public byte League { get; set; }
        public bool InPO { get; set; }
        public short ASST { get; set; }
        public short ERR { get; set; }
        public short PO { get; set; }

        public float Fld
        {
            get => (PO + ASST + ERR > 0 ? (float)Math.Round((PO + ASST) / (float) (PO + ASST + ERR),3) : 0F);
            protected set { }
        }

        public short Ch
        {
            get => (short) (PO + ASST + ERR);
            protected set {}
        }

        public short G { get; set; }

        public Team Team { get; set; }
    }
}
