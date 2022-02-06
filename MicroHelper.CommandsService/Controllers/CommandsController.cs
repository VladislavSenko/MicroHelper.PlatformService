using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using MicroHelper.CommandsService.Dtos;
using MicroHelper.CommandsService.Infrastructure.Models;
using MicroHelper.CommandsService.Infrastructure.Repositories.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace MicroHelper.CommandsService.Controllers
{
    [Route("api/c/[controller]/{platformId}")]
    [ApiController]
    public class CommandsController : ControllerBase
    {
        private readonly ICommandRepository _commandRepository;
        private readonly IPlatformRepository _platformRepository;
        private readonly IMapper _mapper;

        public CommandsController(ICommandRepository commandRepository, 
            IPlatformRepository platformRepository,
            IMapper mapper)
        {
            _commandRepository = commandRepository;
            _platformRepository = platformRepository;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<List<Command>>> GetAllCommandsForPlatform(int platformId)
        {
            var commands = await _commandRepository.GetAllCommandsForPlatformAsync(platformId);
            if (!commands.Any())
            {
                return NotFound();
            }

            var commandReadDtos = _mapper.Map<CommandReadDto>(commands);
            return Ok(commandReadDtos);
        }

        [HttpGet("{commandId}")]
        public async Task<ActionResult<Command>> GetCommandForPlatform(int platformId, int commandId)
        {
            var command = await _commandRepository.GetCommandAsync(platformId, commandId);
            if (command == null)
            {
                return NotFound();
            }

            var commandReadDto = _mapper.Map<CommandReadDto>(command);
            return Ok(commandReadDto);
        }

        [HttpPost]
        public async Task<ActionResult<Command>> CreateCommandForPlatform(int platformId, CommandCreateDto commandCreateDto)
        {
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
