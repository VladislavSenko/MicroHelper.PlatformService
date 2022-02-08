using RabbitMQ.Client;

namespace MicroHelper.PlatformService.Services.Interfaces
{
    public interface IMessageBusFactory
    {
        public ConnectionFactory GetConnectionFactory();
    }
}
