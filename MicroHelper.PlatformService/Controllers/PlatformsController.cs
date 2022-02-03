using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MicroHelper.PlatformService.Dtos;
using MicroHelper.PlatformService.Infrastructure.Models;
using MicroHelper.PlatformService.Infrastructure.Repositories.Interfaces;
using MicroHelper.PlatformService.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace MicroHelper.PlatformService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PlatformsController : ControllerBase
    {
        private readonly IPlatformRepository _repository;
        private readonly IMapper _mapper;
        private readonly ICommandHttpClient _commandHttpClient;

        public PlatformsController(
            IPlatformRepository repository,
            IMapper mapper,
            ICommandHttpClient commandHttpClient)
        {
            _repository = repository;
            _mapper = mapper;
            _commandHttpClient = commandHttpClient;
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
            if (platformModel == null)
            {
                return NotFound();
            }

            var platformReadDto = _mapper.Map<PlatformReadDto>(platformModel);
            return Ok(platformReadDto);
        }

        [HttpPost]
        public async Task<ActionResult<PlatformReadDto>> CreatePlatform(PlatformCreateDto platformCreateDto, CancellationToken cancellationToken)
        {
            var platformModel = _mapper.Map<Platform>(platformCreateDto);
            await _repository.CreatePlatformAsync(platformModel);
            await _repository.SaveChangesAsync();
            var platformReadDto = _mapper.Map<PlatformReadDto>(platformModel);

            try
            {
                var sendPlatformResponse = await _commandHttpClient.SendPlatformToCommandAsync(platformReadDto, cancellationToken);
                Console.WriteLine($"{DateTime.Now} => response: {sendPlatformResponse}");
            }
            catch (Exception e)
            {
                Console.WriteLine($"{DateTime.Now} => something wrong with sending synchronously: {e.Message}");
            }

            return CreatedAtRoute(nameof(GetPlatformById), new {platformReadDto.Id }, platformReadDto);
        }
    }
}
