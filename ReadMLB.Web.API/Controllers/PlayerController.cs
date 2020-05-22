using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using ReadMLB.Services;
using ReadMLB.Web.API.Model;

namespace ReadMLB.Web.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PlayersController : ControllerBase
    {
        private readonly IPlayersService _playersService;
        private readonly IMapper _mapper;

        public PlayersController(IPlayersService playersService, IMapper mapper)
        {
            _playersService = playersService;
            _mapper = mapper;
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetPlayerAsync([FromRoute] long id, [FromQuery] short year, [FromQuery] bool inPO)
        {
            var player = await _playersService.GetByIdAsync(id, year, inPO);
            return Ok(_mapper.Map<PlayerModel>(player));
        }
    }
}