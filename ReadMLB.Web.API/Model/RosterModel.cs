using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ReadMLB.Web.API.Model
{
    public class RosterModel
    {
        public byte Slot { get; set; }
        public long PlayerId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public byte? Shirt { get; set; }

        public string PrimaryPosition { get; set; }
        public string SecondaryPosition { get; set; }

        public string Bats { get; set; }
        public string Throws { get; set; }
    }
}
