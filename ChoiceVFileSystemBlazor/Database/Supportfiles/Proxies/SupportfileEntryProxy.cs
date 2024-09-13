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
            .Include(x => x.FileUploads)
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
    
    public async Task<SupportfileFileUploadDbModel?> GetFileAsync(Ulid id)
    {
        await using var dbContext = await dbContextFactory.CreateDbContextAsync();
        return await dbContext.SupportfileFileUploadDbModels
            .AsNoTracking()
            .Include(x => x.EntryModel)
            .FirstOrDefaultAsync(x => x.Id == id);
    }

    public int GetMaxFileSize() => 1048576 * 5;
    
    public async Task<bool> AddFileAsync(SupportfileFileUploadDbModel file, Ulid supportfileId, Ulid accessId)
    {
        await using var dbContext = await dbContextFactory.CreateDbContextAsync();
        
        switch (file.ContentType)
        {
            case "application/pdf":
            case "image/png":
            case "image/jpg":
            case "image/jpeg":
                break;
            default:
                return false;
        }

        if (file.Data.Length > GetMaxFileSize())
        {
            return false;
        }
        
        await dbContext.SupportfileFileUploadDbModels.AddAsync(file);
        await supportfileLogsProxy.AddLogWithoutSaveAsync(dbContext, new(
            supportfileId,
            SupportfileLogTypeEnum.AddFileUpload,
            accessId,
            $"FileId: {file.Id} \n" +
            $"FileName: {file.FileName} \n " +
            $"ContentType: {file.ContentType} \n " +
            $"FileSize: {file.SizeText}"
        ));
        var changes = await dbContext.SaveChangesAsync();
        
        return changes > 0;
    }

    public async Task<bool> RenameFileAsync(Ulid id, string newName, Ulid supportfileId, Ulid accessId)
    {
        var file = await GetFileAsync(id);
        if (file is null) return false;
        
        await using var dbContext = await dbContextFactory.CreateDbContextAsync();

        var oldName = file.FileName;
        file.FileName = newName;
        
        dbContext.SupportfileFileUploadDbModels.Update(file);
        await supportfileLogsProxy.AddLogWithoutSaveAsync(dbContext, new(
            supportfileId,
            SupportfileLogTypeEnum.DeleteFileUpload,
            accessId,
            $"FileId: {file.Id} \n" +
            $"OldName: {oldName} \n " +
            $"NewName: {file.FileName} \n "
        ));
        var changes = await dbContext.SaveChangesAsync();
        
        return changes > 0;
    }

    public async Task<bool> DeleteFileAsync(Ulid id, Ulid supportfileId, Ulid accessId)
    {
        var file = await GetFileAsync(id);
        if (file is null) return false;
        
        await using var dbContext = await dbContextFactory.CreateDbContextAsync();
        
        dbContext.SupportfileFileUploadDbModels.Remove(file);
        await supportfileLogsProxy.AddLogWithoutSaveAsync(dbContext, new(
            supportfileId,
            SupportfileLogTypeEnum.DeleteFileUpload,
            accessId,
            $"FileId: {file.Id} \n" +
            $"FileName: {file.FileName} \n " +
            $"ContentType: {file.ContentType} \n "
        ));
        var changes = await dbContext.SaveChangesAsync();
        
        return changes > 0;
    } 
}