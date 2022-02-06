using System.Collections.Generic;
using System.Threading.Tasks;
using MicroHelper.CommandsService.Infrastructure.Models;

namespace MicroHelper.CommandsService.Infrastructure.Repositories.Interfaces
{
    public interface IPlatformRepository
    {
        Task<List<Platform>> GetAllPlatformsAsync();
        Task CreatePlatformAsync(Platform platform);
        Task<bool> CheckIsPlatformExistByIdAsync(int platformId);
        Task<bool> SaveChangesAsync();
    }
}
