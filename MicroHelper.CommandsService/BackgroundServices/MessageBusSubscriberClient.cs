using System;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using MicroHelper.CommandsService.Configuration;
using MicroHelper.CommandsService.Services.Interfaces;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace MicroHelper.CommandsService.BackgroundServices
{
    public class MessageBusSubscriberClient : BackgroundService
    {
        private readonly IMessageBusProcessorService _messageBusProcessorService;
        private readonly IAppConfiguration _appConfiguration;
        private readonly ILogger<MessageBusSubscriberClient> _logger;
        private IConnection _rabbitMqConnection;
        private IModel _rabbitMqChannel;
        private string _rabbitMqQueueName;

        public MessageBusSubscriberClient(
            IMessageBusProcessorService messageBusProcessorService,
            IAppConfiguration appConfiguration,
            ILogger<MessageBusSubscriberClient> logger)
        {
            _messageBusProcessorService = messageBusProcessorService;
            _appConfiguration = appConfiguration;
            _logger = logger;

            InitializeRabbitMq();
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            stoppingToken.ThrowIfCancellationRequested();
            var consumer = new EventingBasicConsumer(_rabbitMqChannel);
            consumer.Received += async (sender, args) =>
            {
                var body = args.Body;
                var message = Encoding.UTF8.GetString(body.ToArray());
                await _messageBusProcessorService.ProcessMessageAsync(message);
            };
            _rabbitMqChannel.BasicConsume(queue: _rabbitMqQueueName, autoAck: true, consumer: consumer);

            return Task.CompletedTask;
        }

        private void InitializeRabbitMq()
        {
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
                _rabbitMqQueueName = _rabbitMqChannel.QueueDeclare().QueueName;
                _rabbitMqChannel.QueueBind(queue:_rabbitMqQueueName,
                    exchange: _appConfiguration.RabbitMqExchangeName,
                    routingKey: string.Empty);
                _rabbitMqConnection.ConnectionShutdown += RabbitMqConnectionOnConnectionShutdown;

                _logger.LogInformation($"{DateTime.Now} => rabbitMQ is listened");
            }
            catch (Exception e)
            {
                _logger.LogCritical($"{DateTime.Now} => rabbitMQ couldn't create a connection. {e.Message}");
            }
        }

        private void RabbitMqConnectionOnConnectionShutdown(object? sender, ShutdownEventArgs e)
        {
            _logger.LogInformation($"{DateTime.Now} => rabbitMQ connection is shutdown");
        }

        public override void Dispose()
        {
            base.Dispose();

            if (_rabbitMqChannel.IsOpen)
            {
                _rabbitMqChannel.Close();
                _rabbitMqConnection.Close();
            }
        }
    }
}
