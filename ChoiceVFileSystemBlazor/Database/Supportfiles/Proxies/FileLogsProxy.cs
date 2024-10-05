using ChoiceVFileSystemBlazor.Database.Supportfiles.DbModels;
using ChoiceVFileSystemBlazor.Database.Supportfiles.Proxies.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace ChoiceVFileSystemBlazor.Database.Supportfiles.Proxies;

public class FileLogsProxy(IDbContextFactory<ChoiceVFileSystemBlazorDatabaseContext> dbContextFactory) : IFileLogsProxy
{
    public async Task<List<FileLogsDbModel>> GetAllLogsForSupportfileIdAsync(Ulid id)
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
    public async Task<FileLogsDbModel?> AddLogAsync(FileLogsDbModel fileLog)
    {
        await using var dbContext = await dbContextFactory.CreateDbContextAsync();
        
        await dbContext.SupportfileLogsDbModels.AddAsync(fileLog);
        var changes = await dbContext.SaveChangesAsync();
        
        return changes <= 0 ? null : fileLog;
    }

    public async Task<bool> AddLogWithoutSaveAsync(ChoiceVFileSystemBlazorDatabaseContext dbContext, FileLogsDbModel fileLog)
    {
        await dbContext.SupportfileLogsDbModels.AddAsync(fileLog);
        return true;
    }
}