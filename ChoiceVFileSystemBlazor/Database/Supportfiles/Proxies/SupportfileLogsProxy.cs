using ChoiceVFileSystemBlazor.Database.Supportfiles.DbModels;
using ChoiceVFileSystemBlazor.Database.Supportfiles.Proxies.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace ChoiceVFileSystemBlazor.Database.Supportfiles.Proxies;

public class SupportfileLogsProxy(IDbContextFactory<ChoiceVFileSystemBlazorDatabaseContext> dbContextFactory) : ISupportfileLogsProxy
{
    public async Task<List<SupportfileLogsDbModel>> GetAllLogsForSupportfileIdAsync(Ulid id)
    {
        await using var dbContext = await dbContextFactory.CreateDbContextAsync();

        var list = await dbContext.SupportfileLogsDbModels
            .Include(x => x.AccessModel)
            .AsNoTracking()
            .Where(x => x.SupportfileId == id)
            .OrderByDescending(x => x.Id)
            .ToListAsync();

        return list;
    }

    // Return null if adding failed
    public async Task<SupportfileLogsDbModel?> AddLogAsync(SupportfileLogsDbModel supportfileLog)
    {
        await using var dbContext = await dbContextFactory.CreateDbContextAsync();
        
        await dbContext.SupportfileLogsDbModels.AddAsync(supportfileLog);
        var changes = await dbContext.SaveChangesAsync();
        
        return changes <= 0 ? null : supportfileLog;
    }

    public async Task<bool> AddLogWithoutSaveAsync(ChoiceVFileSystemBlazorDatabaseContext dbContext, SupportfileLogsDbModel supportfileLog)
    {
        await dbContext.SupportfileLogsDbModels.AddAsync(supportfileLog);
        return true;
    }
}