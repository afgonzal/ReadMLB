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
    [Route("api/defense")]
    [ApiController]
    public class DefenseStatsController : ControllerBase
    {
        private readonly IDefenseStatsService _defenseService;
        private readonly IMapper _mapper;

        public DefenseStatsController(IDefenseStatsService defenseService, IMapper mapper)
        {
            _defenseService = defenseService;
            _mapper = mapper;
        }


        [HttpGet("{playerId:int}")]
        public async Task<IActionResult> GetPlayerDefenseStatsAsync([FromRoute] long playerId,
            [FromQuery] bool inPO = false)
        {
            var stats = await _defenseService.GetPlayerDefenseStatsAsync(playerId, inPO);
            return Ok(_mapper.Map<IEnumerable<DefenseStatModel>>(stats));
        }
    }
}