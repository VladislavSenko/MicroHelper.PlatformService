using System;
using System.Threading.Tasks;
using MicroHelper.CommandsService.BackgroundServices;
using MicroHelper.CommandsService.Configuration;
using MicroHelper.CommandsService.Infrastructure.Data;
using MicroHelper.CommandsService.Infrastructure.Repositories.Implementation;
using MicroHelper.CommandsService.Infrastructure.Repositories.Interfaces;
using MicroHelper.CommandsService.Services.Implementation;
using MicroHelper.CommandsService.Services.Interfaces;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;

namespace MicroHelper.CommandsService
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();

            services.AddHostedService<MessageBusSubscriberClient>();
            
            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
            services.AddDbContext<AppDbContext>(options => options.UseInMemoryDatabase("InMem"));
           
            services.AddScoped<ICommandRepository, CommandRepository>();
            services.AddScoped<IPlatformRepository, PlatformRepository>();
            services.AddScoped<IGrpcPlatformService, Services.Implementation.GrpcPlatformService>();

            services.AddSingleton<IMessageBusProcessorService, MessageBusProcessorService>();
            services.AddSingleton<IAppConfiguration, AppConfiguration>();

            services.AddLogging();

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "MicroHelper.CommandsService", Version = "v1" });
            });
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "MicroHelper.CommandsService v1"));
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            Task.Run(async () =>
            {
                await app.SeedDataAsync();
            });
        }

        public static readonly ILoggerFactory SqlLoggerFactory = LoggerFactory.Create(builder =>
        {
            builder.AddConsole();
        });
    }
}
