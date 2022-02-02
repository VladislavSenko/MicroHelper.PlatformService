using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using MicroHelper.PlatformService.Dtos;
using MicroHelper.PlatformService.Infrastructure.Models;
using MicroHelper.PlatformService.Infrastructure.Repositories.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace MicroHelper.PlatformService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PlatformsController : ControllerBase
    {
        private readonly IPlatformRepository _repository;
        private readonly IMapper _mapper;

        public PlatformsController(
            IPlatformRepository repository,
            IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<List<PlatformReadDto>>> GetPlatforms()
        {
            var platformModels = await _repository.GetAllAsync();
            var platformReadDtos = _mapper.Map<List<PlatformReadDto>>(platformModels);

            return Ok(platformReadDtos);
        }

        [HttpGet("{id}", Name = "GetPlatformById")]
        public async Task<ActionResult<PlatformReadDto>> GetPlatformById(int id)
        {
            var platformModel = await _repository.GetByIdAsync(id);
            if (platformModel != null)
            {
                var platformReadDto = _mapper.Map<PlatformReadDto>(platformModel);
                return Ok(platformReadDto);
            }

            return NotFound();
        }

        [HttpPost]
        public async Task<ActionResult<PlatformReadDto>> CreatePlatform(PlatformCreateDto platformCreateDto)
        {
            var platformModel = _mapper.Map<Platform>(platformCreateDto);
            await _repository.CreatePlatformAsync(platformModel);
            await _repository.SaveChangesAsync();

            var platformReadDto = _mapper.Map<PlatformReadDto>(platformModel);

            return CreatedAtRoute(nameof(GetPlatformById), new {platformReadDto.Id }, platformReadDto);
        }
    }
}
