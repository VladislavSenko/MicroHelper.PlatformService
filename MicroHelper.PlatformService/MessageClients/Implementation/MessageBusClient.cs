using System;
using System.Text;
using System.Text.Json;
using MicroHelper.Common.Dtos;
using MicroHelper.PlatformService.Configuration;
using MicroHelper.PlatformService.MessageClients.Interfaces;
using Microsoft.Extensions.Logging;
using RabbitMQ.Client;

namespace MicroHelper.PlatformService.MessageClients.Implementation
{
    public class MessageBusClient : IMessageBusClient
    {
        private readonly IConnection _rabbitMqConnection;
        private readonly IModel _rabbitMqChannel;
        private readonly ILogger<MessageBusClient> _logger;
        private readonly IAppConfiguration _appConfiguration;

        public MessageBusClient(ILogger<MessageBusClient> logger,
            IAppConfiguration appConfiguration)
        {
            _logger = logger;
            _appConfiguration = appConfiguration;
            var messageBusConnectionFactory = new ConnectionFactory
            {
                HostName = _appConfiguration.RabbitMqUrl,
                Port = _appConfiguration.RabbitMqMessagePort
            };

            try
            {
                _rabbitMqConnection = messageBusConnectionFactory.CreateConnection();
                _rabbitMqChannel = _rabbitMqConnection.CreateModel();
                _rabbitMqChannel.ExchangeDeclare(exchange: _appConfiguration.RabbitMqExchangeName, type: ExchangeType.Fanout);
                _rabbitMqConnection.ConnectionShutdown += RabbitMqConnectionOnConnectionShutdown;
            }
            catch (Exception e)
            {
                _logger.LogCritical($"{DateTime.Now} => rabbitMQ couldn't create a connection. {e.Message}");
            }
        }

        private void RabbitMqConnectionOnConnectionShutdown(object? sender, ShutdownEventArgs e)
        { 
            _logger.LogInformation($"{DateTime.Now} => rabbitMQ connection is shutdown");
            Dispose();
        }

        public void PublishPlatform(PlatformPublishedToBusDto platformPublishedToBusDto)
        {
            var messageJson = JsonSerializer.Serialize(platformPublishedToBusDto);
            if (_rabbitMqConnection is {IsOpen: true})
            {
                SendMessageToRabbitMq(messageJson);
            }
            else
            {
                _logger.LogWarning($"{DateTime.Now} => rabbitMq connection closed");
            }
        }

        private void SendMessageToRabbitMq(string messageJson)
        {
            var messageBytes = Encoding.UTF8.GetBytes(messageJson);
            if (_rabbitMqChannel is {IsOpen: true})
            {
                _rabbitMqChannel.BasicPublish(exchange: _appConfiguration.RabbitMqExchangeName,
                    routingKey:"test_route",
                    basicProperties: null,
                    body:messageBytes);
                _logger.LogInformation($"{DateTime.Now} we have sent {messageJson}");
            }
            else
            {
                _logger.LogWarning($"{DateTime.Now} => rabbitMq channel closed");
            }
        }

        private void Dispose()
        {
            if (_rabbitMqChannel.IsOpen)
            {
                _rabbitMqChannel.Close();
                _rabbitMqConnection.Close();
            }
        }
    }
}
