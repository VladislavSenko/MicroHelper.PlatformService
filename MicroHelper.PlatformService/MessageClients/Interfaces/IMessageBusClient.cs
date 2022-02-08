using MicroHelper.Common.Dtos;
using MicroHelper.PlatformService.Dtos;

namespace MicroHelper.PlatformService.MessageClients.Interfaces
{
    public interface IMessageBusClient
    {
        public void PublishPlatform(PlatformPublishedToBusDto platformPublishedToBusDto);
    }
}
