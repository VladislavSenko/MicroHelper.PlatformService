using MicroHelper.PlatformService.Constants;
using Microsoft.Extensions.Configuration;

namespace MicroHelper.PlatformService.Configuration
{
    public class AppConfiguration : IAppConfiguration
    {
        private readonly IConfiguration _configuration;
        public AppConfiguration(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public string CommandsServiceBaseUrl => _configuration.GetValue<string>(AppSettingConstants.CommandsServiceBaseUrl);
        public int RabbitMqMessagePort => int.Parse(_configuration.GetValue<string>(AppSettingConstants.RabbitMqMessagePortSettingName));
        public string RabbitMqUrl => _configuration.GetValue<string>(AppSettingConstants.RabbitMqUrlSettingName);
        public string RabbitMqExchangeName => _configuration.GetValue<string>(AppSettingConstants.RabbitMqExchangeSettingName);
    }
}
