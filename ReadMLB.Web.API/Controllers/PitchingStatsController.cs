using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using ReadMLB.Services;
using ReadMLB.Web.API.Model;

namespace ReadMLB.Web.API.Controllers
{
    [Route("api/pitching")]
    [ApiController]
    public class PitchingStatsController : ControllerBase
    {
        private readonly IPitchingService _pitchingService;
        private readonly IMapper _mapper;

        public PitchingStatsController(IPitchingService pitchingService, IMapper mapper)
        {
            _pitchingService = pitchingService;
            _mapper = mapper;
        }

        [HttpGet("{playerId:int}")]
        public async Task<IActionResult> GetPlayerPitchingStatsAsync([FromRoute] long playerId, [FromQuery] bool inPO = false)
        {
            var stats = await _pitchingService.GetPlayerPitchingStatsAsync(playerId, inPO);
            return Ok(_mapper.Map<IEnumerable<PitchingStatModel>>(stats));
        }
        [HttpGet("league/{league:int}/{year:int}")]
        public async Task<IActionResult> GetLeagueBattingStatsAsync([FromRoute] byte league, [FromRoute] short year, [FromQuery]bool inPO = false, [FromQuery]byte? teamId = null, [FromQuery]int take = 500)
        {
            var result = await _pitchingService.GetLeaguePitchingStatsLeadersAsync(league, year, inPO, teamId, take);
            return Ok(_mapper.Map<IEnumerable<PitchingAndPlayerStatModel>>(result));
        }
    }
}