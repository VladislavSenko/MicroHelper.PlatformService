namespace MicroHelper.PlatformService.Configuration
{
    public interface IAppConfiguration
    {
        public string CommandsServiceBaseUrl { get; }
        public int RabbitMqMessagePort { get; }
        public string RabbitMqUrl { get; }
        public string RabbitMqExchangeName { get; }
    }
}
