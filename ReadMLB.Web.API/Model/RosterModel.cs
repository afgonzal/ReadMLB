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
        public string PlayerFirstName { get; set; }
        public string PlayerLastName { get; set; }

        public string PlayerPosition { get; set; }
        public string PlayerSecondaryPosition { get; set; }
    }
}
