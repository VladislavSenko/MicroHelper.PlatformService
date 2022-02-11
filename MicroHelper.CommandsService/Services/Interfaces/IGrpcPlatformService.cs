using System.Collections.Generic;
using MicroHelper.CommandsService.Infrastructure.Models;

namespace MicroHelper.CommandsService.Services.Interfaces
{
    public interface IGrpcPlatformService
    {
        public List<Platform> GetAllPlatformsAsync();
    }
}
