using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MicroHelper.CommandsService.Infrastructure.Data;
using MicroHelper.CommandsService.Infrastructure.Models;
using MicroHelper.CommandsService.Infrastructure.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace MicroHelper.CommandsService.Infrastructure.Repositories.Implementation
{
    public class CommandRepository : ICommandRepository
    {
        private readonly AppDbContext _dbContext;

        public CommandRepository(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task CreateCommandAsync(int platformId, Command command)
        {
            if (command == null)
            {
                throw new ArgumentNullException(nameof(command));
            }
            
            command.PlatformId = platformId;
            await _dbContext.Commands.AddAsync(command);
        }

        public async Task<Command> GetCommandAsync(int platformId, int commandId)
        {
            var command =  await  _dbContext.Commands.FirstOrDefaultAsync(c => c.PlatformId == platformId &&
                c.Id == commandId);
            return command;
        }

        public async Task<List<Command>> GetAllCommandsForPlatformAsync(int platformId)
        {
            var commands = await _dbContext.Commands.Where(c => c.PlatformId == platformId)
                .OrderBy(c => c.Platform.Name)
                .ToListAsync();
            return commands;
        }

        public async Task<bool> SaveChangesAsync()
        {
            var saveChangesResult = await _dbContext.SaveChangesAsync() > 0;
            return saveChangesResult;
        }
    }
}
