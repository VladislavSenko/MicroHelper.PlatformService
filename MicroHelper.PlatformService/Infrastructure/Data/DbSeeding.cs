using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MicroHelper.PlatformService.Infrastructure.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace MicroHelper.PlatformService.Infrastructure.Data
{
    public static class DbSeeding
    {
        public static async Task SeedAsync(this IApplicationBuilder app)
        {
            using var scopeService = app.ApplicationServices.CreateScope();
            var dbContext = scopeService.ServiceProvider.GetService<AppDbContext>();
            
            if (dbContext == null)
            {
                throw new ArgumentNullException(nameof(dbContext));
            }

            await InsertTempDataAsync(dbContext);
        }

        private static async Task InsertTempDataAsync(AppDbContext dbContext)
        {
            if (dbContext.Platforms.Any())
            {
                Console.WriteLine($"{DateTime.Now} => data is exist");
            }
            else
            {
                await dbContext.AddRangeAsync(new List<Platform>
                {
                    new()
                    {
                        Name = "dot net",
                        Publisher = "Microsoft",
                        Cost = "free"
                    },
                    new()
                    {
                        Name = "java",
                        Publisher = "Oracle",
                        Cost = "100"
                    },
                    new()
                    {
                        Name = "datagrip",
                        Publisher = "jetbrains",
                        Cost = "120"
                    }
                });
                await dbContext.SaveChangesAsync();
                Console.WriteLine($"{DateTime.Now} => date  inserted");
            }
        }
    }
}
