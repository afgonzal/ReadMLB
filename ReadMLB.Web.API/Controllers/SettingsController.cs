using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using ReadMLB.Services;
using ReadMLB.Web.API.Model;

namespace ReadMLB.Web.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SettingsController : ControllerBase
    {
        private readonly ITeamsService _teamsService;
        private readonly IConfiguration _config;
        private readonly IMapper _mapper;

        public SettingsController(ITeamsService teamsService, IMapper mapper, IConfiguration config)
        {
            _teamsService = teamsService;
            _config = config;
            _mapper = mapper;
        }

        public async Task<IActionResult> GetSettings()
        {
            var organization = await _teamsService.GetOrganizationById(Convert.ToByte(_config["CurrentTeam"]));
            var settings = new SettingsModel
            {
                InPO = Convert.ToBoolean(_config["inPO"]),
                Year = Convert.ToInt16(_config["CurrentYear"]),
                Teams = _mapper.Map<ICollection<TeamModel>>(organization)
            };
            return Ok(settings);
        }
    }
}