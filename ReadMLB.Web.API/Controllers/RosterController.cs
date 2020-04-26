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
    public class RosterController : ControllerBase
    {
        private readonly IRostersService _rosterService;
        private readonly IMapper _mapper;

        public RosterController(IRostersService rostersService, IMapper mapper)
        {
            _rosterService = rostersService;
            _mapper = mapper;
        }

        [HttpGet("{teamId:int:max(101)}/{year:int:min(2004):max(2050)}/{inPO:bool}")]
        public async Task<IActionResult> GetTeamRoster(byte teamId, short year, bool inPO)
        {
            var roster = await _rosterService.GetTeamRosterAsync(teamId, year, inPO);
            return Ok(_mapper.Map<ICollection<RosterModel>>(roster));
        }
    }
}