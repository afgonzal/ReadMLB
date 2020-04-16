
using System.Collections.Generic;

namespace ReadMLB.Entities
{
    public class Player
    {
        public int PlayerNumerator { get; set; }
        public long PlayerId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }

        public ICollection<Batting> BattingStats { get; set; }
    }
}
