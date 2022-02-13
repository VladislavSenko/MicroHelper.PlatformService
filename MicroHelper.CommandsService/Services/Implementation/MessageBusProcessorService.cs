    using System;
using System.Text.Json;
using System.Threading.Tasks;
using AutoMapper;
using MicroHelper.CommandsService.Dtos;
using MicroHelper.CommandsService.Infrastructure.Models;
using MicroHelper.CommandsService.Infrastructure.Repositories.Interfaces;
using MicroHelper.CommandsService.Services.Interfaces;
using MicroHelper.Common.Dtos;
using MicroHelper.Common.Emuns;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace MicroHelper.CommandsService.Services.Implementation
{
    public class MessageBusProcessorService : IMessageBusProcessorService
    {
        private readonly IMapper _mapper;
        private readonly IServiceScopeFactory _serviceScopeFactory;
        private readonly ILogger<MessageBusProcessorService> _logger;

        public MessageBusProcessorService(IMapper mapper,
            IServiceScopeFactory serviceScopeFactory,
            ILogger<MessageBusProcessorService> logger)
        {
            _mapper = mapper;
            _serviceScopeFactory = serviceScopeFactory;
            _logger = logger;
        }

        public async Task ProcessMessageAsync(string message)
        {
            var messageEventType = JsonSerializer.Deserialize<GenericEventDto>(message);
            switch (messageEventType?.EventType)
            {
                case MessageBusEventType.PlatformPublished:
                    await AddPlatformAsync(message);
                    break;
                case MessageBusEventType.Undetermined:
                    _logger.LogWarning($"{DateTime.Now} => undetermined eventType is detected");
                    break;
                case null:
                    _logger.LogWarning($"{DateTime.Now} => null eventType is detected");
                    break;
            }
        }

        private async Task AddPlatformAsync(string message)
        {
            using var scope = _serviceScopeFactory.CreateScope();
            var platformRepository = scope.ServiceProvider.GetRequiredService<IPlatformRepository>();
            var platformPublishedToToBusDto = JsonSerializer.Deserialize<PlatformPublishedToBusDto>(message);
            var platform = _mapper.Map<Platform>(platformPublishedToToBusDto);

            if (await platformRepository.CheckIsPlatformExistByIdAsync(platform.PlatformExternalId))
            {
                _logger.LogWarning($"{DateTime.Now} => platform with id {platform.PlatformExternalId} is exist");
            }
            else
            {
                await platformRepository.CreatePlatformAsync(platform);
                await platformRepository.SaveChangesAsync();
                _logger.LogInformation($"{DateTime.Now} => platform is published with id {platform.Id}");
            }
        }
    }
}
