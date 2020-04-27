
using System.Collections.Generic;
using System.ComponentModel;

namespace ReadMLB.Entities
{
    public class Player
    {
        public int PlayerNumerator { get; set; }
        public long PlayerId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }

        public ICollection<Batting> BattingStats { get; set; }

        public short Year { get; set; }

        public long EAId { get; set; }

        public bool IsInvalid { get; set; }

        public PlayerPositionAbr? PrimaryPosition { get; set; }
        public PlayerPositionAbr? SecondaryPosition { get; set; }
        public byte? Shirt { get; set; }
        public Bats? Bats { get; set; }
        public ThrowHand? Throws { get; set; }
    }

    public enum Bats
    {
        [Description("Right")]
        R,
        [Description("Left")]
        L, 
        [Description("Switch")]
        S
    }

    public enum ThrowHand
    {
        [Description("Right")]
        R, 
        [Description("Left")]
        L
    }
}
