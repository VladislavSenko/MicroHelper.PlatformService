using System;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using MicroHelper.PlatformService.Configuration;
using MicroHelper.PlatformService.Constants;
using MicroHelper.PlatformService.Dtos;
using MicroHelper.PlatformService.Helpers;
using MicroHelper.PlatformService.Services.Interfaces;

namespace MicroHelper.PlatformService.Services.Implementation
{
    public class CommandHttpClient : ICommandHttpClient
    {
        private readonly HttpClient _httpClient;
        private readonly IAppConfiguration _appConfiguration;

        public CommandHttpClient(HttpClient httpClient, IAppConfiguration appConfiguration)
        {
            _httpClient = httpClient;
            _appConfiguration = appConfiguration;
        }

        public async Task<string> SendPlatformToCommandAsync(PlatformReadDto platformReadDto, CancellationToken cancellationToken)
        {
            var sendPlatformRequest = new StringContent(JsonSerializer.Serialize(platformReadDto),
                Encoding.UTF8, AppConstants.JsonContentType);
            var sendPlatformResponse = await _httpClient.PostAsync(UrlHelper.Combine(_appConfiguration.CommandsServiceBaseUrl, AppConstants.PlatformCreateCommandUrl),
                sendPlatformRequest, cancellationToken);

            if (sendPlatformResponse.IsSuccessStatusCode)
            {
                var sendPlatformResponseText = await sendPlatformResponse.Content.ReadAsStringAsync(cancellationToken);
                return sendPlatformResponseText;
            }

            return string.Empty;
        }
    }
}
