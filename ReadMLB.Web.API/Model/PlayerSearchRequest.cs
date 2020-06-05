using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ReadMLB.Entities;

namespace ReadMLB.Web.API.Model
{
    public class PlayerSearchRequest
    {
        public byte? League { get; set; }
        public short? Year { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public PlayerPositionAbr Position { get; set; }
    }
}
