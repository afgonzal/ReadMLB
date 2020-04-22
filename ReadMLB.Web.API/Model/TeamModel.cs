﻿namespace ReadMLB.Web.API.Model
{
    public class TeamModel
    {
        public byte TeamId { get; set; }
        public byte League { get; set; }
        public string TeamName { get; set; }
        public string TeamAbr { get; set; }
        public byte OrganizationId { get; set; }
        public string Organization { get; set; }
    }
}