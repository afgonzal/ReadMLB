using System;
using System.Collections.Generic;
using System.Text;

namespace ReadMLB.Entities
{
    public class Division
    {
        public byte League { get; set; }
        public byte DivisionId { get; set; }

        public string Name
        {
            get
            {
                switch (League)
                {
                    case 0:
                        return DivisionId switch
                        {
                            0 => "AL West",
                            1 => "AL Central",
                            2 => "AL East",
                            3 => "NL West",
                            4 => "NL Central",
                            5 => "NL East",
                            _ => throw new ArgumentException("Invalid Division for MLB"),
                        };
                    case 1:
                        return DivisionId switch
                        {
                            6 => "IL West",
                            7 => "IL North",
                            8 => "IL South",
                            9 => "PCL Am. North",
                            10 => "PCL Am. South",
                            11 => "PCL Pac. North",
                            12 => "PCL Pac. South",
                            _ => throw new ArgumentException("Invalid Division for AAA"),
                        };
                    case 2:
                        return DivisionId switch
                        {
                            13 => "EL Northern",
                            14 => "EL Southern",
                            15 => "SL South",
                            16 => "SL North",
                            17 => "TL South",
                            18 => "TL North",
                            _ => throw new ArgumentException("Invalid Division for AA"),
                        };
                    default:
                        throw new ArgumentException("Invalid league");
                }
            }
        }

        public ICollection<Team> Teams { get; set; }
    }
}
