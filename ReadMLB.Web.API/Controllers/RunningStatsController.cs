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
    [Route("api/running")]
    [ApiController]
    public class RunningStatsController : ControllerBase
    {
        private readonly IRunningStatsService _runningStatsService;
        private readonly IMapper _mapper;

        public RunningStatsController(IRunningStatsService runningStatsService, IMapper mapper)
        {
            _runningStatsService = runningStatsService;
            _mapper = mapper;
        }

        [HttpGet("{playerId:int}")]

        public async Task<IActionResult> GetPlayerRunningStatsAsync([FromRoute] long playerId, [FromQuery]bool inPO = false)
        {
            var stats = await _runningStatsService.GetPlayerRunningStatsAsync(playerId, inPO);
            return Ok(_mapper.Map<IEnumerable<RunningStatModel>>(stats));
        }
    }
}