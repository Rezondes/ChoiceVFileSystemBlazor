using ChoiceVFileSystemBlazor.Database.Accesses.DbModels;
using ChoiceVFileSystemBlazor.Database.Accesses.Proxies.Interfaces;
using ChoiceVFileSystemBlazor.Database.Supportfiles.DbModels;
using Microsoft.EntityFrameworkCore;

namespace ChoiceVFileSystemBlazor.Database.Accesses.Proxies;

public class AccessLogsProxy(IDbContextFactory<ChoiceVFileSystemBlazorDatabaseContext> dbContextFactory) : IAccessLogsProxy
{
    public async Task<List<AccessLogsDbModel>> GetAllAsync()
    {
        await using var dbContext = await dbContextFactory.CreateDbContextAsync();
        
        return await dbContext.AccessLogsDbModels
            .AsNoTracking()
            .Include(x => x.AccessModel)
            .Include(x => x.TargetAccessModel)
            .ToListAsync();
    }

    // Return null if adding failed
    public async Task<AccessLogsDbModel?> AddLogAsync(AccessLogsDbModel accessLog)
    {
        await using var dbContext = await dbContextFactory.CreateDbContextAsync();
        
        await dbContext.AccessLogsDbModels.AddAsync(accessLog);
        var changes = await dbContext.SaveChangesAsync();
        
        return changes <= 0 ? null : accessLog;
    }

    public async Task<bool> AddLogWithoutSaveAsync(ChoiceVFileSystemBlazorDatabaseContext dbContext, AccessLogsDbModel accessLog)
    {
        await dbContext.AccessLogsDbModels.AddAsync(accessLog);
        return true;
    }
}