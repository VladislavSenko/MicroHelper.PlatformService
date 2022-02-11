using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MicroHelper.CommandsService.Infrastructure.Models;
using MicroHelper.CommandsService.Infrastructure.Repositories.Interfaces;
using MicroHelper.CommandsService.Services.Interfaces;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace MicroHelper.CommandsService.Infrastructure.Data
{
    public static class DbSeeding
    {
        public static async Task SeedDataAsync(this IApplicationBuilder app)
        {
            using var scopeService = app.ApplicationServices.CreateScope();
            var grpcService = scopeService.ServiceProvider.GetService<IGrpcPlatformService>()!;
            var platforms = grpcService.GetAllPlatformsAsync()!;

            if (platforms.Any())
            {
                var platformRepository = scopeService.ServiceProvider.GetService<IPlatformRepository>();
                await InsertPlatformsAsync(platforms, platformRepository);
            }
        }

        private static async Task InsertPlatformsAsync(List<Platform> platforms, IPlatformRepository platformRepository)
        {
            foreach (var platform in platforms)
            {
                if (!await platformRepository.CheckIsPlatformExistByIdAsync(platform.PlatformExternalId))
                {
                    await platformRepository.CreatePlatformAsync(platform);
                }

                await platformRepository.SaveChangesAsync();
            }
        }
    }
}
