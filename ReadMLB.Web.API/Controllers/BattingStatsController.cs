using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using ReadMLB.Services;
using System.Threading.Tasks;

namespace ReadMLB.Web.API.Controllers
{
    [Route("api/batting")]
    [ApiController]
    public class BattingStatsController : ControllerBase
    {
        private IBattingService _battingService;
        private readonly IConfiguration _configuration;

        public BattingStatsController(IBattingService battingService, IConfiguration config)
        {
            _battingService = battingService;
            _configuration = config;
        }

        [HttpGet]
        public async Task<IActionResult> GetCurrentTeamBatters()
        {
            var result = await _battingService.GetTeamBattersAsync(short.Parse(_configuration["CurrentYear"]), byte.Parse(_configuration["CurrentTeam"]));
            return Ok(result);
        }

        [HttpGet("{year:int}/{team:int}")]
        public async Task<IActionResult> GetTeamBatters(short year, byte team)
        {
            var result = await _battingService.GetTeamBattersAsync(year, team);
            return Ok(result);
        }
    }
}