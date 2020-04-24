using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ReadMLB.Web.API.Model
{
    public class SettingsModel
    {
        public short Year { get; set; }
        public bool InPO { get; set; }
        public ICollection<TeamModel> Teams { get; set; }
    }
}
