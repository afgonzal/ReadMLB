﻿
using System;

namespace ReadMLB.Entities
{
    public class Batting
    {
        public long BattingId { get; set; }
        public long PlayerId { get; set; }
        public short Year { get; set; }
        public byte League { get; set; }
        public short TeamId { get; set; }
        public bool InPO { get; set; }
        public BattingVs BattingVs{ get;set; }
        public short G { get; set; }
        public short PA { get; set; }
        public short H1B { get; set; }
        public short H2B { get; set; }
        public short H3B { get; set; }
        public short HR { get; set; }
        public short RBI { get; set; }
        public short SO { get; set; }
        public short BB { get; set; }
        public short SH { get; set; }

        public short SF { get; set; }

        public short HBP { get; set; }
        public short IBB { get; set; }

        public short H
        {
            get
            {
                return (short)(H1B + H2B + H3B + HR);
            }
            protected set { }

        }
        public short TB
        {
            get { return (short)(H1B + H2B * 2 + H3B * 3 + HR * 4); }
            protected set { }
        }

        public short AB
        {
            get
            {
                return (short)(PA - BB - SH - SF - HBP);
            }
            protected set { }
        }
        public short XBH
        {
            get
            {
                return (short)(H2B+H3B+HR);
            }
            protected set { }
        }
        public float BBK
        {
            get
            {
                return SO>0 ? (float)Math.Round((float)BB/(float)SO,3) : 0;
            }
            protected set { }
        }

        public float BA
        {
            get
            {
                return AB>0 ? (float)Math.Round((float)H/(float)AB,3) : 0;
            }
            protected set { }
        }

        public float SLG
        {
            get
            {
                return AB > 0 ? (float)Math.Round((float)TB / (float)AB, 3) : 0;
            }
            protected set { }
        }

        public float ABHR
        {
            get
            {
                return HR > 0 ? (float)Math.Round((float)AB / (float)HR, 3) : 0;
            }
            protected set { }
        }

        public float OBP
        {
            get
            {
                return AB + BB + HBP + SF > 0 ? (float)Math.Round((float)(H + BB + HBP) / (float)(AB + BB + HBP + SF), 3) : 0;
            }
            protected set { }
        }

        public float OPS
        {
            get
            {
                return (float)Math.Round(OBP + SLG, 3);
            }
            protected set { }
        }

    }
}