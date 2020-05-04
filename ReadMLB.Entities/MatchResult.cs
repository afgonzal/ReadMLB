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
        public byte League { get; set; }

        public override string ToString()
        {
            return $"{Round} {DateTime.ToString("dd/MM/yyyy")} {HomeTeamId} vs {AwayTeamId} {HomeScore}-{AwayScore}";
        }

        public char WoL(byte teamId)
        {
            if (HomeTeamId == teamId)
            {
                if (HomeScore > AwayScore)
                    return 'W';
                return 'L';
            }
            else if (AwayTeamId == teamId)
            {
                if (AwayScore > HomeScore)
                    return 'W';
                return 'L';
            }
            throw new ArgumentException($"Team {teamId} not involved in match {MatchId}");
        }
    }
}
