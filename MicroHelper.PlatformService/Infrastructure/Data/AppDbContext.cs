using MicroHelper.PlatformService.Infrastructure.Models;
using Microsoft.EntityFrameworkCore;

namespace MicroHelper.PlatformService.Infrastructure.Data
{
    public class AppDbContext : DbContext
    {
        public DbSet<Platform> Platforms { get; set; }

        public AppDbContext(DbContextOptions options) : base(options)
        {
        }
    }
}
