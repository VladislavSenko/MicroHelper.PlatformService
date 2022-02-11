using AutoMapper;
using MicroHelper.CommandsService.Dtos;
using MicroHelper.CommandsService.Infrastructure.Models;
using MicroHelper.Common;
using MicroHelper.Common.Dtos;

namespace MicroHelper.CommandsService.AutoMapper
{
    public class PlatformProfile : Profile
    {
        public PlatformProfile()
        {
            CreateMap<Platform, PlatformReadDto>();
            CreateMap<PlatformPublishedToBusDto, Platform>()
                .ForMember(p => p.PlatformExternalId,
                    opt => opt.MapFrom(p => p.Id));
            CreateMap<GrpcPlatformModel, Platform>()
                .ForMember(p => p.PlatformExternalId, opt => opt.MapFrom(p => p.PlatformId));
        }
    }
}
