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
    }
}
