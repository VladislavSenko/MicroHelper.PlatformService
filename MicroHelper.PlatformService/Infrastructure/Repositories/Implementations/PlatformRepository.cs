using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MicroHelper.PlatformService.Infrastructure.Data;
using MicroHelper.PlatformService.Infrastructure.Models;
using MicroHelper.PlatformService.Infrastructure.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace MicroHelper.PlatformService.Infrastructure.Repositories.Implementations
{
    public class PlatformRepository : IPlatformRepository
    {
        private readonly AppDbContext _dbContext;
        public PlatformRepository(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task<bool> SaveChangesAsync()
        {
            var saveChangesResult = await _dbContext.SaveChangesAsync();
            return saveChangesResult > 0;
        }

        public async Task<List<Platform>> GetAllAsync()
        {
            var platforms = await _dbContext.Platforms.ToListAsync();
            return platforms;
        }

        public async Task<Platform> GetByIdAsync(int id)
        {
            var platform = await _dbContext.Platforms.FirstOrDefaultAsync(p => p.Id == id);
            return platform;
        }

        public async Task CreatePlatformAsync(Platform platform)
        {
            if (platform == null)
            {
                throw new ArgumentNullException(nameof(platform));
            }
         
            await _dbContext.AddAsync(platform);
        }
    }
}
