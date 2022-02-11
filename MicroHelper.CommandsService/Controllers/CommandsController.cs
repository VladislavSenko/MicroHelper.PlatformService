using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using MicroHelper.CommandsService.Dtos;
using MicroHelper.CommandsService.Infrastructure.Models;
using MicroHelper.CommandsService.Infrastructure.Repositories.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace MicroHelper.CommandsService.Controllers
{
    [Route("api/c/[controller]")]
    [ApiController]
    public class CommandsController : ControllerBase
    {
        private readonly ICommandRepository _commandRepository;
        private readonly IPlatformRepository _platformRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<CommandsController> _logger;

        public CommandsController(ICommandRepository commandRepository, 
            IPlatformRepository platformRepository,
            IMapper mapper,
            ILogger<CommandsController> logger)
        {
            _commandRepository = commandRepository;
            _platformRepository = platformRepository;
            _mapper = mapper;
            _logger = logger;
        }

        [HttpGet]
        public ActionResult Index()
        {
            return Ok("TEST RESULT");
        }

        [HttpGet("{platformId}")]
        public async Task<ActionResult<List<Command>>> GetAllCommandsForPlatform(int platformId)
        {
            _logger.LogInformation($"{DateTime.Now} => {nameof(GetAllCommandsForPlatform)}");
            var commands = await _commandRepository.GetAllCommandsForPlatformAsync(platformId);
            var commandReadDtos = _mapper.Map<List<CommandReadDto>>(commands);
            return Ok(commandReadDtos);
        }

        [HttpGet("{platformId}/{commandId}", Name = nameof(GetCommandForPlatform))]
        public async Task<ActionResult<Command>> GetCommandForPlatform(int platformId, int commandId)
        {
            _logger.LogInformation($"{DateTime.Now} => {nameof(GetCommandForPlatform)}");
            var command = await _commandRepository.GetCommandAsync(platformId, commandId);
            if (command == null)
            {
                return NotFound();
            }

            var commandReadDto = _mapper.Map<CommandReadDto>(command);
            return Ok(commandReadDto);
        }

        [HttpPost("{platformId}")]
        public async Task<ActionResult<Command>> CreateCommandForPlatform(int platformId, CommandCreateDto commandCreateDto)
        {
            _logger.LogInformation($"{DateTime.Now} => {nameof(CreateCommandForPlatform)}");
            var isPlatformExist = await _platformRepository.CheckIsPlatformExistByIdAsync(platformId);
            if (!isPlatformExist)
            {
                return NotFound();
            }

            var command = _mapper.Map<Command>(commandCreateDto);
            await _commandRepository.CreateCommandAsync(platformId, command);
            await _commandRepository.SaveChangesAsync();
            var commandReadDto = _mapper.Map<CommandReadDto>(command);

            return CreatedAtRoute(nameof(GetCommandForPlatform),
                new {platformId, commandId = command.Id}, commandReadDto);
        }
    }
}
