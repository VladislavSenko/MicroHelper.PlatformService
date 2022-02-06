using AutoMapper;
using MicroHelper.CommandsService.Dtos;
using MicroHelper.CommandsService.Infrastructure.Models;

namespace MicroHelper.CommandsService.AutoMapper
{
    public class CommandProfile : Profile
    {
        public CommandProfile()
        {
            CreateMap<CommandCreateDto, Command>();
            CreateMap<Command, CommandReadDto>();
        }
    }
}
