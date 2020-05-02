using System;

namespace ReadMLB.Entities
{
    public class MatchResult
    {
        public long MatchId { get; set; }
        public short Year { get; set; }
        public bool InPO{ get; set; }
        public string Round { get; set; }
        public byte HomeTeamId { get; set; }
        public byte AwayTeamId { get; set; }
        public byte HomeScore{ get; set; }
        public byte AwayScore { get; set; }
        public DateTime DateTime { get; set; }

    }
}
