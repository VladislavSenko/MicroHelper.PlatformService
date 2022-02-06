using MicroHelper.CommandsService.Infrastructure.Models;
using Microsoft.EntityFrameworkCore;

namespace MicroHelper.CommandsService.Infrastructure.Data
{
    public class AppDbContext :DbContext
    {
        public DbSet<Platform> Platforms { get; set; }
        public DbSet<Command> Commands { get; set; }

        public AppDbContext(DbContextOptions options) : base(options)
        {
        }
    }
}
