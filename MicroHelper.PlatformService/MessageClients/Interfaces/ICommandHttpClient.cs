using System.Threading;
using System.Threading.Tasks;
using MicroHelper.PlatformService.Dtos;

namespace MicroHelper.PlatformService.MessageClients.Interfaces
{
    public interface ICommandHttpClient
    {
        Task<string> SendPlatformToCommandAsync(PlatformReadDto platformReadDto, CancellationToken cancellationToken);
    }
}
