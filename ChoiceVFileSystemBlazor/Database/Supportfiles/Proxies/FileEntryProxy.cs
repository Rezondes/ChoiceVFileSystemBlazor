using ChoiceVFileSystemBlazor.Database.Supportfiles.DbModels;
using ChoiceVFileSystemBlazor.Database.Supportfiles.Enums;
using ChoiceVFileSystemBlazor.Database.Supportfiles.Proxies.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace ChoiceVFileSystemBlazor.Database.Supportfiles.Proxies;

public class FileEntryProxy(IDbContextFactory<ChoiceVFileSystemBlazorDatabaseContext> dbContextFactory, IFileLogsProxy fileLogsProxy) : IFileEntryProxy
{
    public async Task<List<FileEntryDbModel>> GetAllEntrysForSupportfileIdAsync(Ulid id)
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

    public async Task<FileEntryDbModel?> GetAsync(Ulid id)
    {
        await using var dbContext = await dbContextFactory.CreateDbContextAsync();
        
        return await dbContext.SupportfileEntryDbModels
            .AsNoTracking()
            .Include(x => x.CreatorAccessModel)
            .FirstOrDefaultAsync(x => x.Id == id);
    }

    // Return null if adding failed
    public async Task<FileEntryDbModel?> AddEntryAsync(FileEntryDbModel fileEntry)
    {
        await using var dbContext = await dbContextFactory.CreateDbContextAsync();
        
        await dbContext.SupportfileEntryDbModels.AddAsync(fileEntry);
        await fileLogsProxy.AddLogWithoutSaveAsync(dbContext, new(
            fileEntry.SupportfileId,
            SupportfileLogTypeEnum.AddEntry,
            fileEntry.CreatedByAccessId,
            $"Id: {fileEntry.Id}"
        ));
        
        var changes = await dbContext.SaveChangesAsync();
        
        return changes <= 0 ? null : fileEntry;
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
        await fileLogsProxy.AddLogWithoutSaveAsync(dbContext, new(
            entry.SupportfileId,
            SupportfileLogTypeEnum.ModifyEntry,
            accessId,
            $"Id: {entry.Id} \n\n" +
            $"OldContent: {oldContent} \n\n" +
            $"NewContent: {newContent}"
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
        await fileLogsProxy.AddLogWithoutSaveAsync(dbContext, new(
            entry.SupportfileId,
            SupportfileLogTypeEnum.RemoveEntry,
            accessId,
            $"Id: {entry.Id} \n\n" +
            $"Content: {entry.Content} \n\n" +
            $"Creator: {entry.CreatedByAccessId}"
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
        await fileLogsProxy.AddLogWithoutSaveAsync(dbContext, new(
            entry.SupportfileId,
            SupportfileLogTypeEnum.RestoreEntry,
            accessId,
            $"Id: {entry.Id} \n\n" +
            $"Content: {entry.Content} \n\n" +
            $"Creator: {entry.CreatedByAccessId}"
        ));
        var changes = await dbContext.SaveChangesAsync();
        
        return changes > 0;
    }
    
    public async Task<FileUploadDbModel?> GetFileAsync(Ulid id)
    {
        await using var dbContext = await dbContextFactory.CreateDbContextAsync();
        return await dbContext.SupportfileFileUploadDbModels
            .AsNoTracking()
            .Include(x => x.EntryModel)
            .FirstOrDefaultAsync(x => x.Id == id);
    }

    public long GetMaxFileSize() => 10 * 1024 * 1024;
    
    public async Task<bool> AddFileAsync(FileUploadDbModel file, Ulid supportfileId, Ulid accessId)
    {
        await using var dbContext = await dbContextFactory.CreateDbContextAsync();
        
        switch (file.ContentType)
        {
            case "image/png":
            case "image/jpg":
            case "image/jpeg":
            case "application/pdf":
                break;
            default:
                return false;
        }

        if (file.Data.Length > GetMaxFileSize())
        {
            return false;
        }
        
        await dbContext.SupportfileFileUploadDbModels.AddAsync(file);
        await fileLogsProxy.AddLogWithoutSaveAsync(dbContext, new(
            supportfileId,
            SupportfileLogTypeEnum.AddFileUpload,
            accessId,
            $"FileId: {file.Id} \n\n" +
            $"FileName: {file.FileName} \n\n" +
            $"ContentType: {file.ContentType} \n\n" +
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
        await fileLogsProxy.AddLogWithoutSaveAsync(dbContext, new(
            supportfileId,
            SupportfileLogTypeEnum.DeleteFileUpload,
            accessId,
            $"FileId: {file.Id} \n\n" +
            $"OldName: {oldName} \n\n" +
            $"NewName: {file.FileName}"
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
        await fileLogsProxy.AddLogWithoutSaveAsync(dbContext, new(
            supportfileId,
            SupportfileLogTypeEnum.DeleteFileUpload,
            accessId,
            $"FileId: {file.Id} \n\n" +
            $"FileName: {file.FileName} \n\n" +
            $"ContentType: {file.ContentType}"
        ));
        var changes = await dbContext.SaveChangesAsync();
        
        return changes > 0;
    } 
}