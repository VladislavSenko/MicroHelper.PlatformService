using System;
using System.Threading.Tasks;
using MicroHelper.PlatformService.Configuration;
using MicroHelper.PlatformService.Constants;
using MicroHelper.PlatformService.Infrastructure.Data;
using MicroHelper.PlatformService.Infrastructure.Repositories.Implementations;
using MicroHelper.PlatformService.Infrastructure.Repositories.Interfaces;
using MicroHelper.PlatformService.Services.Implementation;
using MicroHelper.PlatformService.Services.Interfaces;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;

namespace MicroHelper.PlatformService
{
    public class Startup
    {
        public IConfiguration Configuration { get; }
        private readonly IWebHostEnvironment _hostEnvironment;
        public Startup(IConfiguration configuration,
            IWebHostEnvironment hostEnvironment)
        {
            Configuration = configuration;
            _hostEnvironment = hostEnvironment;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            if (_hostEnvironment.IsProduction())
            {
                services.AddDbContext<AppDbContext>(options =>
                {
                    options.UseSqlServer(Configuration.GetConnectionString(AppSettingConstants.ConnectionStringSettingName));
                    options.UseLoggerFactory(SqlLoggerFactory);
                });
            }
            else
            {
                services.AddDbContext<AppDbContext>(options =>
                {
                    options.UseInMemoryDatabase("InMem");
                    options.UseLoggerFactory(SqlLoggerFactory);
                });
            }
            
            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

            services.AddHttpClient<ICommandHttpClient, CommandHttpClient>(); 

            services.AddSingleton<IAppConfiguration, AppConfiguration>();
            
            services.AddScoped<IPlatformRepository, PlatformRepository>();

            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "MicroHelper.PlatformService", Version = "v1" });
            });

            Console.WriteLine($"{DateTime.Now} => commands service url: {Configuration.GetValue<string>(AppSettingConstants.CommandsServiceBaseUrl)}");
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseDeveloperExceptionPage();
            app.UseSwagger();
            app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "MicroHelper.PlatformService v1"));

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            Task.Run(async () =>
            {
                await app.SeedAsync(_hostEnvironment.IsProduction());
            });
        }

        public static readonly ILoggerFactory SqlLoggerFactory = LoggerFactory.Create(builder =>
        {
            builder.AddConsole();
        });
    }
}
