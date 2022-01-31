using System.Collections.Generic;
using System.Threading.Tasks;
using MicroHelper.PlatformService.Infrastructure.Models;

namespace MicroHelper.PlatformService.Infrastructure.Repositories.Interfaces
{
    public interface IPlatformRepository
    {
        Task<bool> SaveChangesAsync();
        Task<List<Platform>> GetAllAsync();
        Task<Platform> GetByIdAsync(int id);
        Task CreatePlatformAsync(Platform platform);
    }
}
