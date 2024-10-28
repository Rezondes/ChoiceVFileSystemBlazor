using ChoiceVFileSystemBlazor.Database._Shared;
using ChoiceVFileSystemBlazor.Database.Supportfiles.DbModels;
using ChoiceVFileSystemBlazor.Database.Supportfiles.Enums;
using ChoiceVFileSystemBlazor.Database.Supportfiles.Proxies.Interfaces;
using ChoiceVFileSystemBlazor.Services;
using Microsoft.EntityFrameworkCore;

namespace ChoiceVFileSystemBlazor.Database.Supportfiles.Proxies;

public class FileProxy(IDbContextFactory<ChoiceVFileSystemBlazorDatabaseContext> dbContextFactory, IFileLogsProxy fileLogsProxy, IFileCategoryProxy fileCategoryProxy, LockService lockService) : IFileProxy
{
    public async Task<List<FileDbModel>> GetAllGroupingfilesAsync()
    {
        await using var dbContext = await dbContextFactory.CreateDbContextAsync();
        
        return await dbContext.SupportfileDbModels
            .Where(x => x.Type == FileTypeEnum.Groupingfile)
            .Include(x => x.CreatorAccessModel)
            .Include(x => x.Category)
            .ToListAsync();
    }
    
    public async Task<List<FileDbModel>> GetAllFullGroupingfilesAsync()
    {
        await using var dbContext = await dbContextFactory.CreateDbContextAsync();
        
        // welcome in the including-hell
        return await dbContext.SupportfileDbModels
            .Where(x => x.Type == FileTypeEnum.Groupingfile)
            .Include(x => x.CreatorAccessModel)
            .Include(x => x.Category)
            .Include(x => x.CharacterEntrys)
            .Include(x => x.Entrys)
            .ThenInclude(x => x.FileUploads)
            .Include(x => x.Entrys)
            .ThenInclude(x => x.CreatorAccessModel)
            .Include(x => x.Logs)
            .ThenInclude(x => x.AccessModel)
            .AsSplitQuery()
            .ToListAsync();
    }
    
    public async Task<List<FileDbModel>> GetAllSupportfilesAsync()
    {
        await using var dbContext = await dbContextFactory.CreateDbContextAsync();
        
        return await dbContext.SupportfileDbModels
            .Where(x => x.Type == FileTypeEnum.Supportfile)
            .Include(x => x.CreatorAccessModel)
            .Include(x => x.Category)
            .ToListAsync();
    }
    
    public async Task<List<FileDbModel>> GetAllFullSupportfilesAsync()
    {
        await using var dbContext = await dbContextFactory.CreateDbContextAsync();
        
        // welcome in the including-hell
        return await dbContext.SupportfileDbModels
            .Where(x => x.Type == FileTypeEnum.Supportfile)
            .Include(x => x.CreatorAccessModel)
            .Include(x => x.Category)
            .Include(x => x.CharacterEntrys)
            .Include(x => x.Entrys)
                .ThenInclude(x => x.FileUploads)
            .Include(x => x.Entrys)
                .ThenInclude(x => x.CreatorAccessModel)
            .Include(x => x.Logs)
                .ThenInclude(x => x.AccessModel)
            .AsSplitQuery()
            .ToListAsync();
    }

    public async Task<FileDbModel?> GetAsync(Ulid id)
    {
        await using var dbContext = await dbContextFactory.CreateDbContextAsync();
        
        return await dbContext.SupportfileDbModels
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Id == id);
    }
    
    public async Task<FileDbModel?> GetFullAsync(Ulid id)
    {
        await using var dbContext = await dbContextFactory.CreateDbContextAsync();
        
        return await dbContext.SupportfileDbModels
            .Include(x => x.CreatorAccessModel)
            .Include(x => x.CharacterEntrys)
            .Include(x => x.Entrys)
                .ThenInclude(x => x.FileUploads)
            .Include(x => x.Entrys)
                .ThenInclude(x => x.CreatorAccessModel)
            .Include(x => x.Logs)
                .ThenInclude(x => x.AccessModel)
            .AsSplitQuery()
            .FirstOrDefaultAsync(x => x.Id == id);
    }

    // Return null if adding failed
    public async Task<FileDbModel?> AddAsync(FileDbModel file)
    {
        return await lockService.LockAsync(async () =>
        {
            await using var dbContext = await dbContextFactory.CreateDbContextAsync();
        
            var currentUtcYear = DateTime.UtcNow.Year;
        
            switch (file.Type)
            {
                case FileTypeEnum.Supportfile:
                    var allSupportFiles = await GetAllSupportfilesAsync();
                    var supportfilesCount = allSupportFiles.Count(x => x.CreatedAt.Year == currentUtcYear) + 1;
                    file.DisplayId = $"sf_{currentUtcYear}_{supportfilesCount:D4}";
                    break;
                
                case FileTypeEnum.Groupingfile:
                    var allGroupingfiles = await GetAllGroupingfilesAsync();
                    var groupingfilesCount = allGroupingfiles.Count(x => x.CreatedAt.Year == currentUtcYear) + 1;
                    file.DisplayId = $"gf_{currentUtcYear}_{groupingfilesCount:D4}";
                    break;
                
                default:
                    throw new ArgumentOutOfRangeException();
            }
        
            await dbContext.SupportfileDbModels.AddAsync(file);
        
            await fileLogsProxy.AddLogWithoutSaveAsync(dbContext, new(
                file.Id,
                FileLogTypeEnum.AddFile,
                file.CreatedByAccessId,
                string.Empty
            ));
        
            var changes = await dbContext.SaveChangesAsync();
        
            return changes <= 0 ? null : file;
        });
    }

    public async Task<bool> AddCharEntryAsync(FileCharacterEntryDbModel characterEntry, Ulid accessId)
    {
        var fileCheck  = await GetFullAsync(characterEntry.SupportfileId);
        if (fileCheck is null) return false;
        
        await using var dbContext = await dbContextFactory.CreateDbContextAsync();

        var charCheck = dbContext.SupportfileCharacterEntryDbModels
            .FirstOrDefault(x => 
                x.CharacterId == characterEntry.CharacterId && 
                x.SupportfileId == characterEntry.SupportfileId);
        if (charCheck is not null) return false;
        
        dbContext.SupportfileCharacterEntryDbModels.Add(characterEntry);
        await fileLogsProxy.AddLogWithoutSaveAsync(dbContext, new(
            characterEntry.SupportfileId,
            FileLogTypeEnum.AddCharEntry,
            accessId,
            $"Id: {characterEntry.Id} \n\n" +
            $"CharacterId: {characterEntry.CharacterId}"
        ));
        var changes = await dbContext.SaveChangesAsync();
        
        return changes > 0;
    }

    public async Task<bool> RemoveCharEntryAsync(FileCharacterEntryDbModel characterEntry, Ulid accessId)
    {
        var fileCheck  = await GetFullAsync(characterEntry.SupportfileId);
        if (fileCheck is null) return false;
        
        await using var dbContext = await dbContextFactory.CreateDbContextAsync();

        var charCheck = dbContext.SupportfileCharacterEntryDbModels
            .FirstOrDefault(x => 
                x.CharacterId == characterEntry.CharacterId &&
                x.SupportfileId == characterEntry.SupportfileId);
        if (charCheck is null) return false;
        
        dbContext.SupportfileCharacterEntryDbModels.Remove(charCheck);
        await fileLogsProxy.AddLogWithoutSaveAsync(dbContext, new(
            characterEntry.SupportfileId,
            FileLogTypeEnum.RemoveCharEntry,
            accessId,
            $"Id: {characterEntry.Id} \n\n" +
            $"CharacterId: {characterEntry.CharacterId}"
        ));
        var changes = await dbContext.SaveChangesAsync();
        
        return changes > 0;
    }
    
    public async Task<bool> ToggleDeletedAsync(Ulid id, Ulid accessId)
    {
        var file = await GetAsync(id);
        if (file is null) return false;
        
        await using var dbContext = await dbContextFactory.CreateDbContextAsync();

        file.Deleted = !file.Deleted;
        
        var logType = file.Deleted ? FileLogTypeEnum.DeleteFile : FileLogTypeEnum.RestoreFile;
        
        dbContext.SupportfileDbModels.Update(file);
        await fileLogsProxy.AddLogWithoutSaveAsync(dbContext, new(
            file.Id,
            logType,
            accessId,
            string.Empty
        ));
        var changes = await dbContext.SaveChangesAsync();
        
        return changes > 0;
    }

    public async Task<bool> UpdateTitleAsync(Ulid id, string newTitle, Ulid accessId)
    {
        var file = await GetAsync(id);
        if (file is null) return false;
        
        await using var dbContext = await dbContextFactory.CreateDbContextAsync();

        var oldTitle = file.Title;
        
        file.Title = newTitle;
        
        dbContext.SupportfileDbModels.Update(file);
        await fileLogsProxy.AddLogWithoutSaveAsync(dbContext, new(
            file.Id,
            FileLogTypeEnum.ModifyTitle,
            accessId,
            $"OldTitle: {oldTitle} \n\n" +
            $"NewTitle: {file.Title}"
        ));
        var changes = await dbContext.SaveChangesAsync();
        
        return changes > 0;
    }
    
    public async Task<bool> UpdateDescriptionAsync(Ulid id, string newDescription, Ulid accessId)
    {
        var file = await GetAsync(id);
        if (file is null) return false;
        
        await using var dbContext = await dbContextFactory.CreateDbContextAsync();
        
        var oldDescription = file.Description;
        
        file.Description = newDescription; 
        
        dbContext.SupportfileDbModels.Update(file);
        await fileLogsProxy.AddLogWithoutSaveAsync(dbContext, new(
            file.Id,
            FileLogTypeEnum.ModifyDescription,
            accessId,
            $"OldDescription: {oldDescription} \n\n" +
            $"NewDescription: {file.Description}"
        ));
        var changes = await dbContext.SaveChangesAsync();
        
        return changes > 0;
    }
    
    public async Task<bool> UpdateStatusAsync(Ulid id, FileStatusEnum newStatus, Ulid accessId)
    {
        var file = await GetAsync(id);
        if (file is null) return false;
        
        await using var dbContext = await dbContextFactory.CreateDbContextAsync();
        
        var oldStatus = file.Status;
        
        file.Status = newStatus;
        
        dbContext.SupportfileDbModels.Update(file);
        await fileLogsProxy.AddLogWithoutSaveAsync(dbContext, new(
            file.Id,
            FileLogTypeEnum.ModifyStatus,
            accessId,
            $"OldStatus: {oldStatus} \n\n" +
            $"NewStatus: {file.Status}"
        ));
        var changes = await dbContext.SaveChangesAsync();
        
        return changes > 0;
    }
    
    public async Task<bool> UpdateMinRankAsync(Ulid id, RankEnum newMinRank, Ulid accessId)
    {
        var file = await GetAsync(id);
        if (file is null) return false;
        
        await using var dbContext = await dbContextFactory.CreateDbContextAsync();
        
        var oldMinRank = file.MinRank;
        
        file.MinRank = newMinRank;
        
        dbContext.SupportfileDbModels.Update(file);
        await fileLogsProxy.AddLogWithoutSaveAsync(dbContext, new(
            file.Id,
            FileLogTypeEnum.ModifyMinRank,
            accessId,
            $"OldMinRank: {oldMinRank} \n\n" +
            $"NewMinRank: {file.MinRank}"
        ));
        var changes = await dbContext.SaveChangesAsync();
        
        return changes > 0;
    }

    public async Task<bool> ChangeCategoryAsync(Ulid id, Ulid? newCategoryId, Ulid accessId)
    {
        var file = await GetAsync(id);
        if (file is null) return false;

        if (newCategoryId.HasValue)
        {
            var checkCategory = await fileCategoryProxy.GetAsync(newCategoryId.Value);
            if (checkCategory is null) return false;
        }
        
        await using var dbContext = await dbContextFactory.CreateDbContextAsync();
        
        var oldCategoryId = file.CategoryId; 
        file.CategoryId = newCategoryId;
        
        dbContext.SupportfileDbModels.Update(file);
        await fileLogsProxy.AddLogWithoutSaveAsync(dbContext, new(
            file.Id,
            FileLogTypeEnum.ModifyCategory,
            accessId,
            $"OldCategoryId: {oldCategoryId} \n\n" +
                $"NewCategoryId: {file.CategoryId}"
        ));
        var changes = await dbContext.SaveChangesAsync();
        
        return changes > 0;
    }
}