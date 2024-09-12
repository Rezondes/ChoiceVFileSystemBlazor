using ChoiceVFileSystemBlazor.Database.Discord.DbModels;
using ChoiceVFileSystemBlazor.Database.Discord.Proxies.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace ChoiceVFileSystemBlazor.Database.Discord.Proxies;

public class DiscordRoleLogsProxy(IDbContextFactory<ChoiceVFileSystemBlazorDatabaseContext> dbContextFactory) : IDiscordRoleLogsProxy
{
    public async Task<List<DiscordRoleLogsDbModel>> GetAllAsync()
    {
        await using var dbContext = await dbContextFactory.CreateDbContextAsync();
        
        return await dbContext.DiscordRoleLogsDbModels.AsNoTracking().ToListAsync();
    }

    // Return null if adding failed
    public async Task<DiscordRoleLogsDbModel?> AddLogAsync(DiscordRoleLogsDbModel logFile)
    {
        await using var dbContext = await dbContextFactory.CreateDbContextAsync();
        
        await dbContext.DiscordRoleLogsDbModels.AddAsync(logFile);
        var changes = await dbContext.SaveChangesAsync();
        
        return changes <= 0 ? null : logFile;
    }

    public async Task<bool> AddLogWithoutSaveAsync(ChoiceVFileSystemBlazorDatabaseContext dbContext, DiscordRoleLogsDbModel logFile)
    {
        await dbContext.DiscordRoleLogsDbModels.AddAsync(logFile);
        return true;
    }
}