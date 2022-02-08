using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MicroHelper.Common.Dtos;
using MicroHelper.Common.Emuns;
using MicroHelper.PlatformService.Constants;
using MicroHelper.PlatformService.Dtos;
using MicroHelper.PlatformService.Infrastructure.Models;
using MicroHelper.PlatformService.Infrastructure.Repositories.Interfaces;
using MicroHelper.PlatformService.MessageClients.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace MicroHelper.PlatformService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PlatformsController : ControllerBase
    {
        private readonly IPlatformRepository _repository;
        private readonly IMapper _mapper;
        private readonly ICommandHttpClient _commandHttpClient;
        private readonly ILogger<PlatformsController> _logger;
        private readonly IMessageBusClient _messageBusClient;

        public PlatformsController(
            IPlatformRepository repository,
            IMapper mapper,
            ICommandHttpClient commandHttpClient, 
            ILogger<PlatformsController> logger,
            IMessageBusClient messageBusClient)
        {
            _repository = repository;
            _mapper = mapper;
            _commandHttpClient = commandHttpClient;
            _logger = logger;
            _messageBusClient = messageBusClient;
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

            //Send a sync message
            try
            {
                var sendPlatformResponse = await _commandHttpClient.SendPlatformToCommandAsync(platformReadDto, cancellationToken);
                _logger.LogInformation($"{DateTime.Now} => response: {sendPlatformResponse}");
            }
            catch (Exception e)
            {
                _logger.LogCritical($"{DateTime.Now} => something wrong with sending synchronously: {e.Message}");
            }

            //Send an async message
            try
            {
                var platformPublishedToBusDto = _mapper.Map<PlatformPublishedToBusDto>(platformReadDto);
                platformPublishedToBusDto.EventType = MessageBusEventType.PlatformPublished;
                _messageBusClient.PublishPlatform(platformPublishedToBusDto);
            }
            catch (Exception e)
            {
                _logger.LogCritical($"{DateTime.Now} => something wrong with sending asynchronously: {e.Message}");
            }

            return CreatedAtRoute(nameof(GetPlatformById), new {platformReadDto.Id }, platformReadDto);
        }
    }
}
