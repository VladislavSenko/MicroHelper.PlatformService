using System.Threading.Tasks;
using AutoMapper;
using Google.Protobuf.Collections;
using Grpc.Core;
using MicroHelper.Common;
using MicroHelper.PlatformService.Infrastructure.Repositories.Interfaces;

namespace MicroHelper.PlatformService.Services.Implementation
{
    public class GrpcPlatformsService : GrpcPlatformService.GrpcPlatformServiceBase 
    {
        private readonly IMapper _mapper;
        private readonly IPlatformRepository _platformRepository;

        public GrpcPlatformsService(IMapper mapper,
            IPlatformRepository platformRepository)
        {
            _mapper = mapper;
            _platformRepository = platformRepository;
        }

        public override async Task<GrpcPlatformModels> GetAllPlatformsAsync(GetAllPlatformsRequestModel request, ServerCallContext context)
        {
            var platforms = await _platformRepository.GetAllAsync();
            var grpcPlatformModels = new GrpcPlatformModels();
            grpcPlatformModels.Platforms.AddRange(_mapper.Map<RepeatedField<GrpcPlatformModel>>(platforms));

            return grpcPlatformModels;
        }
    }
}
