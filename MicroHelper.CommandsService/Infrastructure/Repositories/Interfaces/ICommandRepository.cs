using System.Collections.Generic;
using System.Threading.Tasks;
using MicroHelper.CommandsService.Infrastructure.Models;

namespace MicroHelper.CommandsService.Infrastructure.Repositories.Interfaces
{
    public interface ICommandRepository
    {
        Task CreateCommandAsync(int platformId, Command command);
        Task<Command> GetCommandAsync(int platformId, int commandId);
        Task<List<Command>> GetAllCommandsForPlatformAsync(int platformId);
        Task<bool> SaveChangesAsync();
    }
}
