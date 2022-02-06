using AutoMapper;
using MicroHelper.CommandsService.Dtos;
using MicroHelper.CommandsService.Infrastructure.Models;

namespace MicroHelper.CommandsService.AutoMapper
{
    public class PlatformProfile : Profile
    {
        public PlatformProfile()
        {
            CreateMap<Platform, PlatformReadDto>();
        }
    }
}
