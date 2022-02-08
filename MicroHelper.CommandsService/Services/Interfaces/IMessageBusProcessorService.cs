using System.Threading.Tasks;

namespace MicroHelper.CommandsService.Services.Interfaces
{
    public interface IMessageBusProcessorService
    {
        Task ProcessMessageAsync(string message);
    }
}
