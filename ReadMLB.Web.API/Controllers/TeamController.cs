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
    }
}