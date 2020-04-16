using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using ReadMLB.Services;
using ReadMLB.Web.API.Model;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ReadMLB.Web.API.Controllers
{
    [Route("api/batting")]
    [ApiController]
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
            return Ok(_mapper.Map<ICollection<BattingStatModel>>(result));
        }

        [HttpGet("{year:int}/{team:int}")]
        public async Task<IActionResult> GetTeamBatters(short year, byte team)
        {
            var result = await _battingService.GetTeamBattersAsync(year, team);
            return Ok(_mapper.Map<ICollection<BattingStatModel>>(result));
        }
    }
}