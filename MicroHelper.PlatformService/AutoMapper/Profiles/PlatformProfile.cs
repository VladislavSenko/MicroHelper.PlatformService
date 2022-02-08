using AutoMapper;
using MicroHelper.Common.Dtos;
using MicroHelper.PlatformService.Dtos;
using MicroHelper.PlatformService.Infrastructure.Models;

namespace MicroHelper.PlatformService.AutoMapper.Profiles
{
    public class PlatformProfile : Profile
    {
        public PlatformProfile()
        {
            CreateMap<Platform, PlatformReadDto>();
            CreateMap<PlatformCreateDto, Platform>();
            CreateMap<PlatformReadDto, PlatformPublishedToBusDto>();
        }
    }
}