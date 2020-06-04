using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ReadMLB.Services;
using ReadMLB.Web.API.Model;

namespace ReadMLB.Web.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TeamsController : ControllerBase
    {
        private readonly ITeamsService _teamsService;
        private readonly IMapper _mapper;
        public TeamsController(ITeamsService teamsService, IMapper mapper)
        {
            _teamsService = teamsService;
            _mapper = mapper;
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetTeam([FromRoute] byte id)
        {
            var team = await _teamsService.GetTeamByIdAsync(id);
            return Ok(_mapper.Map<TeamModel>(team));
        }

        [HttpGet]
        public async Task<IActionResult> GetTeams()
        {
            var teams = await _teamsService.GetTeamsAsync();
            return Ok(_mapper.Map<IEnumerable<TeamModel>>(teams.OrderBy(t => t.League).ThenBy(t => t.Division)));
        }

        [HttpGet("league/{leagueId:int}")]
        public async Task<IActionResult> GetLeagueTeams([FromRoute]byte league)
        {
            var teams = await _teamsService.GetLeagueTeamsAsync(league);
            return Ok(_mapper.Map<IEnumerable<TeamModel>>(teams.OrderBy(t => t.Division)));
        }

        [HttpGet("league/{league:int}/divisions")]
        public async Task<IActionResult> GetLeagueDivisions([FromRoute]byte league)
        {
            var teams = await _teamsService.GetLeagueDivisionsAsync(league);
            return Ok(_mapper.Map<IEnumerable<DivisionModel>>(teams.Where(t => t.League == league).OrderBy(t => t.DivisionId)));
        }

        [HttpGet("organization/{id:int}")]
        public async Task<IActionResult> GetOrganization([FromRoute] byte id)
        {
            var organization = await _teamsService.GetOrganizationById(id);
            return Ok(_mapper.Map<IEnumerable<TeamModel>>(organization));
        }
    }
}