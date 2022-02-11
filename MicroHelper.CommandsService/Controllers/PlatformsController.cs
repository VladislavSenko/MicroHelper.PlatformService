using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using MicroHelper.CommandsService.Dtos;
using MicroHelper.CommandsService.Infrastructure.Repositories.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace MicroHelper.CommandsService.Controllers
{
    [Route("api/c/[controller]")]
    [ApiController]
    public class PlatformsController : ControllerBase
    {
        private readonly IPlatformRepository _platformRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<PlatformsController> _logger;

        public PlatformsController(IPlatformRepository platformRepository,
            IMapper mapper,
            ILogger<PlatformsController> logger)
        {
            _platformRepository = platformRepository;
            _mapper = mapper;
            _logger = logger;
        }    

        [HttpPost]
        public ActionResult Index()
        {
            return Ok("Ok result");
        }

        [HttpGet]
        public async Task<ActionResult<List<PlatformReadDto>>> GetPlatforms()
        {
            _logger.LogInformation($"{DateTime.Now} => {nameof(GetPlatforms)}");
            var platforms = await _platformRepository.GetAllPlatformsAsync();
            var platformReadDtos = _mapper.Map<List<PlatformReadDto>>(platforms);
            return Ok(platformReadDtos);
        }
    }
}
