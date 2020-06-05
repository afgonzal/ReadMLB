using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using ReadMLB.Services;
using ReadMLB.Web.API.Model;

namespace ReadMLB.Web.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PlayersController : ControllerBase
    {
        private readonly IPlayersService _playersService;
        private readonly IMapper _mapper;
        private readonly IConfiguration _config;
        private readonly IRunningStatsService _runningService;
        private readonly IDefenseStatsService _defenseService;

        public PlayersController(IPlayersService playersService, IMapper mapper, IConfiguration config, IRunningStatsService runningService, IDefenseStatsService defenseService)
        {
            _playersService = playersService;
            _mapper = mapper;
            _config = config;
            _runningService = runningService;
            _defenseService = defenseService;
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetPlayerAsync([FromRoute] long id, [FromQuery] bool inPO = false)
        {
            var player = await _playersService.GetByIdAsync(id, inPO);
            return Ok(_mapper.Map<PlayerWithHistoryModel>(player));
        }

        [HttpGet("{id:int}/fieldRunning")]
        public async Task<IActionResult> GetPlayerFieldRunningAsync([FromRoute] long id, [FromQuery] bool inPO = false)
        {
            var runningStats = await _runningService.GetPlayerRunningStatsAsync(id, inPO);
            var defenseStats = await _defenseService.GetPlayerDefenseStatsAsync(id, inPO);
            var stats = from running in runningStats
                join defense in defenseStats on new {running.PlayerId, running.Year, running.TeamId} equals new
                    {defense.PlayerId, defense.Year, defense.TeamId}
                select new FieldRunningStatModel
                {
                    Year = running.Year, League = running.League,
                    Organization = running.Team.Organization != null
                        ? running.Team.Organization.TeamAbr
                        : running.Team.TeamAbr,
                    TeamAbr = running.Team.TeamAbr,
                    InPO = inPO,
                    TeamName = running.Team.TeamName,
                    TeamId = running.TeamId,
                    ASST = defense.ASST,
                    PO = defense.PO,
                    ERR = defense.ERR,
                    Fld = defense.Fld,
                    Ch = defense.Ch,
                    SB = running.SB,
                    CS = running.CS,
                    RS = running.RS
                };
            return Ok(stats);
        }

        [HttpPost("search")]
        public async Task<IActionResult> SearchPlayersAsync([FromBody]PlayerSearchRequest request)
        {
            var players = await _playersService.SearchAsync(request.League, request.Year, request.FirstName,
                request.LastName, request.Position);
            return Ok(_mapper.Map<IEnumerable<PlayerModel>>(players));
        }

        
    }
}