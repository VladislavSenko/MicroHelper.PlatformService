using MicroHelper.PlatformService.Configuration;
using MicroHelper.PlatformService.Services.Interfaces;
using RabbitMQ.Client;

namespace MicroHelper.PlatformService.Services.Implementation
{
    public class MessageBusFactory : IMessageBusFactory
    {
        private readonly ConnectionFactory _connectionFactory;

        public MessageBusFactory(IAppConfiguration appConfiguration)
        {
            _connectionFactory = new ConnectionFactory
            {
                HostName = appConfiguration.RabbitMqUrl,
                Port = appConfiguration.RabbitMqMessagePort
            };
        }

        public ConnectionFactory GetConnectionFactory() => _connectionFactory;
    }
}
