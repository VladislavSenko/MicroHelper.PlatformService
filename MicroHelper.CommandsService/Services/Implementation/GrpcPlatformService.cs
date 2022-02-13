using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using Grpc.Net.Client;
using MicroHelper.CommandsService.Configuration;
using MicroHelper.CommandsService.Infrastructure.Models;
using MicroHelper.CommandsService.Services.Interfaces;
using MicroHelper.Common;
using Microsoft.Extensions.Logging;

namespace MicroHelper.CommandsService.Services.Implementation
{
    public class GrpcPlatformService : IGrpcPlatformService
    {
        private readonly IMapper _mapper;
        private readonly IAppConfiguration _appConfiguration;
        private readonly ILogger<GrpcPlatformService> _logger;

        public GrpcPlatformService(IMapper mapper,
            IAppConfiguration appConfiguration, 
            ILogger<GrpcPlatformService> logger)
        {
            _mapper = mapper;
            _appConfiguration = appConfiguration;
            _logger = logger;
        }

        public List<Platform> GetAllPlatformsAsync()
        {
            var channel = GrpcChannel.ForAddress(_appConfiguration.GrpcPlatformServiceUrl);
            var client = new Common.GrpcPlatformService.GrpcPlatformServiceClient(channel);
            var getAllPlatformsRequestModel = new GetAllPlatformsRequestModel();

            try
            {
                var grpcPlatformModels = client.GetAllPlatformsAsync(getAllPlatformsRequestModel);
                var platforms = _mapper.Map<List<Platform>>(grpcPlatformModels.Platforms.ToList());
                return platforms;
            }
            catch (Exception ex)
            {
                _logger.LogCritical($"{DateTime.Now} => fail {nameof(GetAllPlatformsAsync)} in {nameof(GrpcPlatformService)}. {ex.Message}");
                return new List<Platform>();
            }
        }
    }
}
