namespace MicroHelper.CommandsService.Configuration
{
    public interface IAppConfiguration
    {
        public int RabbitMqMessagePort { get; }
        public string RabbitMqUrl { get; }
        public string RabbitMqExchangeName { get; }
        public string GrpcPlatformServiceUrl { get; }
    }
}
