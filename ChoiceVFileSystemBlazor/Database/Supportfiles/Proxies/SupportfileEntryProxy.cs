using ChoiceVFileSystemBlazor.Database.Supportfiles.DbModels;
using ChoiceVFileSystemBlazor.Database.Supportfiles.Enums;
using ChoiceVFileSystemBlazor.Database.Supportfiles.Proxies.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace ChoiceVFileSystemBlazor.Database.Supportfiles.Proxies;

public class SupportfileEntryProxy(IDbContextFactory<ChoiceVFileSystemBlazorDatabaseContext> dbContextFactory, ISupportfileLogsProxy supportfileLogsProxy) : ISupportfileEntryProxy
{
    public async Task<List<SupportfileEntryDbModel>> GetAllEntrysForSupportfileIdAsync(Ulid id)
    {
        await using var dbContext = await dbContextFactory.CreateDbContextAsync();
        
        var list = await dbContext.SupportfileEntryDbModels
            .AsNoTracking()
            .Include(x => x.CreatorAccessModel)
            .Where(x => x.SupportfileId == id)
            .OrderByDescending(x => x.Id)
            .ToListAsync();

        return list;
    }

    public async Task<SupportfileEntryDbModel?> GetAsync(Ulid id)
    {
        await using var dbContext = await dbContextFactory.CreateDbContextAsync();
        
        return await dbContext.SupportfileEntryDbModels
            .AsNoTracking()
            .Include(x => x.CreatorAccessModel)
            .FirstOrDefaultAsync(x => x.Id == id);
    }

    // Return null if adding failed
    public async Task<SupportfileEntryDbModel?> AddEntryAsync(SupportfileEntryDbModel supportfileEntry)
    {
        await using var dbContext = await dbContextFactory.CreateDbContextAsync();
        
        await dbContext.SupportfileEntryDbModels.AddAsync(supportfileEntry);
        await supportfileLogsProxy.AddLogWithoutSaveAsync(dbContext, new(
            supportfileEntry.SupportfileId,
            SupportfileLogTypeEnum.AddEntry,
            supportfileEntry.CreatedByAccessId,
            $"Id: {supportfileEntry.Id} \n"
        ));
        
        var changes = await dbContext.SaveChangesAsync();
        
        return changes <= 0 ? null : supportfileEntry;
    }

    public async Task<bool> UpdateEntryContentAsync(Ulid id, string newContent, Ulid accessId)
    {
        var entry = await GetAsync(id);
        if (entry is null) return false;
        
        await using var dbContext = await dbContextFactory.CreateDbContextAsync();

        var oldContent = entry.Content;
        
        entry.Content = newContent;
        entry.ModifiedAt = DateTime.UtcNow;

        dbContext.SupportfileEntryDbModels.Update(entry);
        await supportfileLogsProxy.AddLogWithoutSaveAsync(dbContext, new(
            entry.SupportfileId,
            SupportfileLogTypeEnum.ModifyEntry,
            accessId,
            $"Id: {entry.Id} \n" +
            $"OldContent: {oldContent} \n " +
            $"NewContent: {newContent} \n "
        ));
        var changes = await dbContext.SaveChangesAsync();
        
        return changes > 0;
    }

    public async Task<bool> RemoveEntryAsync(Ulid id, Ulid accessId)
    {
        var entry = await GetAsync(id);
        if (entry is null) return false;
        if (entry.Deleted) return false;
        
        await using var dbContext = await dbContextFactory.CreateDbContextAsync();

        entry.Deleted = true;
        
        dbContext.Update(entry);
        await supportfileLogsProxy.AddLogWithoutSaveAsync(dbContext, new(
            entry.SupportfileId,
            SupportfileLogTypeEnum.RemoveEntry,
            accessId,
            $"Id: {entry.Id} \n" +
            $"Content: {entry.Content} \n " +
            $"Creator: {entry.CreatedByAccessId} \n "
        ));
        var changes = await dbContext.SaveChangesAsync();
        
        return changes > 0;
    }

    public async Task<bool> RestoreEntryAsync(Ulid id, Ulid accessId)
    {
        var entry = await GetAsync(id);
        if (entry is null) return false;
        if (!entry.Deleted) return false;
        
        await using var dbContext = await dbContextFactory.CreateDbContextAsync();

        entry.Deleted = false;
        
        dbContext.Update(entry);
        await supportfileLogsProxy.AddLogWithoutSaveAsync(dbContext, new(
            entry.SupportfileId,
            SupportfileLogTypeEnum.RestoreEntry,
            accessId,
            $"Id: {entry.Id} \n" +
            $"Content: {entry.Content} \n " +
            $"Creator: {entry.CreatedByAccessId} \n "
        ));
        var changes = await dbContext.SaveChangesAsync();
        
        return changes > 0;
    }
}