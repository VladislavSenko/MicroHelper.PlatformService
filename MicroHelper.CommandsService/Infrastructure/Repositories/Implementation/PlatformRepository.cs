using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MicroHelper.CommandsService.Infrastructure.Data;
using MicroHelper.CommandsService.Infrastructure.Models;
using MicroHelper.CommandsService.Infrastructure.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace MicroHelper.CommandsService.Infrastructure.Repositories.Implementation
{
    public class PlatformRepository : IPlatformRepository
    {
        private readonly AppDbContext _dbContext;

        public PlatformRepository(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<List<Platform>> GetAllPlatformsAsync()
        {
            var platforms = await _dbContext.Platforms.ToListAsync();
            return platforms;
        }

        public async Task CreatePlatformAsync(Platform platform)
        {
            if (platform == null)
            {
                throw new ArgumentNullException(nameof(platform));
            }

            await _dbContext.AddAsync(platform);
        }

        public async Task<bool> CheckIsPlatformExistByIdAsync(int platformId)
        {
            var platform = await _dbContext.Platforms.SingleOrDefaultAsync(p => p.Id == platformId);
            return platform is not null;
        }

        public async Task<bool> SaveChangesAsync()
        {
            var saveChangesResult = await _dbContext.SaveChangesAsync() >= 0;
            return saveChangesResult;
        }
    }
}
