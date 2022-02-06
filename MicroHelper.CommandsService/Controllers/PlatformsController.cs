using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using MicroHelper.CommandsService.Dtos;
using MicroHelper.CommandsService.Infrastructure.Repositories.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace MicroHelper.CommandsService.Controllers
{
    [Route("api/c/[controller]")]
    [ApiController]
    public class PlatformsController : ControllerBase
    {
        private readonly IPlatformRepository _platformRepository;
        private readonly IMapper _mapper;

        public PlatformsController(IPlatformRepository platformRepository, IMapper mapper)
        {
            _platformRepository = platformRepository;
            _mapper = mapper;
        }    

        [HttpPost]
        public ActionResult Index()
        {
            return Ok("Ok result");
        }

        [HttpGet]
        public async Task<ActionResult<List<PlatformReadDto>>> GetPlatforms()
        {
            var platforms = await _platformRepository.GetAllPlatformsAsync();
            var platformReadDtos = _mapper.Map<PlatformReadDto>(platforms);
            return Ok(platformReadDtos);
        }
    }
}
