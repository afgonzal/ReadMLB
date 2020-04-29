using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using HtmlAgilityPack;
using Microsoft.Extensions.Configuration;
using ReadMLB.Entities;
using ReadMLB.Services;

namespace ReadMLB2020
{
    public class ReadPlayerPositions
    {
        private readonly IPlayersService _playersService;
        private readonly string _htmlSource;
        private readonly ITeamsService _teamsService;
        private readonly IRostersService _rostersService;
        private readonly bool _inPO;
        private readonly short _year;

        public ReadPlayerPositions(IPlayersService playersService, ITeamsService teamsService, IRostersService rostersService, IConfiguration config, short year, bool inPO)
        {
            _playersService = playersService;
            _teamsService = teamsService;
            _rostersService = rostersService;
            _htmlSource = Path.Combine(config["SourceFolder"], $"{year}{(inPO ? 'P' : 'R')}.html");
            _year = year;
            _inPO = inPO;
        }

        public async Task ParseRosterForPlayersAttrssAsync()
        {
            Console.WriteLine("Parsing Rosters for Players Attrs");
            var html = new HtmlDocument();
            html.Load(_htmlSource);
            var teams = await _teamsService.GetTeamsAsync();

            foreach (var team in teams)
            {

                //anchor with teamId is below a b, then there's a br, then a text? then something, then the table
                var rosterTable = html.DocumentNode.SelectSingleNode($"//b/a[@name='t{team.TeamId}']").ParentNode.NextSibling
                    .NextSibling.NextSibling;

                //validate is the roster
                if (rosterTable.FirstChild.FirstChild.InnerHtml != "Roster")
                    throw new FormatException("Roster table not found, or found wrong table.");

                //get team's roster
                var roster = (await _rostersService.GetTeamRosterAsync(team.TeamId, _year, _inPO)).ToList();

                foreach (var row in rosterTable.SelectNodes("./tr").Skip(2))
                {
                    var players = roster.Where(p =>
                        p.Player.FirstName == row.ChildNodes[0].InnerHtml.ExtractName() &&
                        p.Player.LastName == row.ChildNodes[1].InnerHtml.ExtractName());

                    if (players.Count() != 1)
                        Console.WriteLine("Player not found {0} {1} in team {2}",
                            row.ChildNodes[1].InnerHtml.ExtractName(), row.ChildNodes[0].InnerHtml.ExtractName(),
                            team.TeamId);
                    else
                    {
                        var player = players.Single().Player;
                        try
                        {
                            player.Shirt = Convert.ToByte(row.ChildNodes[2].InnerHtml);
                            player.PrimaryPosition =
                                row.ChildNodes[3].InnerHtml.GetEnumFromDescription<PlayerPositionAbr>();
                            player.SecondaryPosition =
                                row.ChildNodes[4].InnerHtml.GetEnumFromDescription<PlayerPositionAbr>();
                            player.Bats = Enum.Parse<Bats>(row.ChildNodes[5].InnerHtml);
                            player.Throws = Enum.Parse<ThrowHand>(row.ChildNodes[6].InnerHtml);
                            await _playersService.UpdatePlayerAttributesAsync(player);
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine("Error");
                        }
                    }
                }
            }
            Console.WriteLine("Parse Players attrs finished.");
        }
    }
}
