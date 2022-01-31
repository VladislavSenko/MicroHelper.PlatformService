using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using MicroHelper.PlatformService.Dtos;
using MicroHelper.PlatformService.Infrastructure.Models;
using MicroHelper.PlatformService.Infrastructure.Repositories.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace MicroHelper.PlatformService.AutoMapper.Profiles
{
    [Route("api/[controller]")]
    [ApiController]
    public class PlatformsController : ControllerBase
    {
        private readonly IPlatformRepository _platformRepository;
        private readonly IMapper _mapper;

        public PlatformsController(IMapper mapper, IPlatformRepository platformRepository)
        {
            _mapper = mapper;
            _platformRepository = platformRepository;
        }

        [HttpGet]
        public async Task<ActionResult<List<PlatformReadDto>>> GetAllPlatforms()
        {
            var platformModels = await _platformRepository.GetAllAsync();
            var platformReadDtos = _mapper.Map<List<PlatformReadDto>>(platformModels);

            return Ok(platformReadDtos);
        }

        [HttpGet("{id}", Name = nameof(GetPlatformById))]
        public async Task<ActionResult<PlatformReadDto>> GetPlatformById(int id)
        {
            var platformModel = await _platformRepository.GetByIdAsync(id);
            if (platformModel == null)
            {
                return NotFound();
            }

            var platformReadDto = _mapper.Map<PlatformReadDto>(platformModel);
            return Ok(platformReadDto);
        }

        [HttpPost]
        public async Task<ActionResult<PlatformReadDto>> CreatePlatform(PlatformCreateDto platformCreateDto)
        {
            var platformModel = _mapper.Map<Platform>(platformCreateDto);
            await _platformRepository.CreatePlatformAsync(platformModel);
            await _platformRepository.SaveChangesAsync();

            var platformReadDto = _mapper.Map<PlatformReadDto>(platformModel);

            return CreatedAtRoute(nameof(GetPlatformById), new {platformModel.Id}, platformReadDto);
        }
    }
}
