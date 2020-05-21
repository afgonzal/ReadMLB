using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.Extensions.Configuration;
using ReadMLB.Services;
using ReadMLB.Web.API.Model;
using Microsoft.AspNetCore.Mvc;

namespace ReadMLB.Web.API.Controllers
{
    [ApiController]
    [Route("api/batting")]
    public class BattingStatsController : ControllerBase
    {
        private IBattingService _battingService;
        private readonly IConfiguration _configuration;
        private readonly IMapper _mapper;

        public BattingStatsController(IBattingService battingService, IMapper mapper, IConfiguration config)
        {
            _battingService = battingService;
            _configuration = config;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> GetCurrentTeamBatters()
        {
            var result = await _battingService.GetTeamBattersAsync(short.Parse(_configuration["CurrentYear"]), byte.Parse(_configuration["CurrentTeam"]));
            return Ok(_mapper.Map<ICollection<BattingAndPlayerStatModel>>(result));
        }

        [Microsoft.AspNetCore.Mvc.HttpGet("{year:int}/{team:int}")]
        public async Task<IActionResult> GetTeamBatters(short year, byte team)
        {
            var result = await _battingService.GetTeamBattersAsync(year, team);
            return Ok(_mapper.Map<ICollection<BattingAndPlayerStatModel>>(result));
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetPlayerBattingStatsAsync([FromRoute] long id, [FromQuery] bool inPO = false)
        {
            var result = await _battingService.GetPlayerBattingStatsAsync(id, inPO);

            return Ok(_mapper.Map<ICollection<BattingStatModel>>(result.OrderBy(b => b.Year).ThenBy(b => b.BattingVs)));
        }
    }
}