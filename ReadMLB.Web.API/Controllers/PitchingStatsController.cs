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
    }
}