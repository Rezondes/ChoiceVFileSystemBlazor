using ChoiceVFileSystemBlazor.Database._Shared;
using ChoiceVFileSystemBlazor.Database.Supportfiles.DbModels;
using ChoiceVFileSystemBlazor.Database.Supportfiles.Enums;
using ChoiceVFileSystemBlazor.Database.Supportfiles.Proxies.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace ChoiceVFileSystemBlazor.Database.Supportfiles.Proxies;

public class SupportfileProxy(IDbContextFactory<ChoiceVFileSystemBlazorDatabaseContext> dbContextFactory, ISupportfileLogsProxy supportfileLogsProxy, ISupportfileCategoryProxy supportfileCategoryProxy) : ISupportfileProxy
{
    public async Task<List<SupportfileDbModel>> GetAllAsync()
    {
        await using var dbContext = await dbContextFactory.CreateDbContextAsync();
        
        return await dbContext.SupportfileDbModels
            .AsNoTracking()
            .Include(x => x.CreatorAccessModel)
            .Include(x => x.Category)
            .ToListAsync();
    }
    
    public async Task<List<SupportfileDbModel>> GetAllFullAsync()
    {
        await using var dbContext = await dbContextFactory.CreateDbContextAsync();
        
        // welcome in the including-hell
        return await dbContext.SupportfileDbModels
            .AsNoTracking()
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

    public async Task<SupportfileDbModel?> GetAsync(Ulid id)
    {
        await using var dbContext = await dbContextFactory.CreateDbContextAsync();
        
        return await dbContext.SupportfileDbModels
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Id == id);
    }
    
    public async Task<SupportfileDbModel?> GetFullAsync(Ulid id)
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
    public async Task<SupportfileDbModel?> AddAsync(SupportfileDbModel file)
    {
        await using var dbContext = await dbContextFactory.CreateDbContextAsync();
        
        await dbContext.SupportfileDbModels.AddAsync(file);
        await supportfileLogsProxy.AddLogWithoutSaveAsync(dbContext, new(
            file.Id,
            SupportfileLogTypeEnum.AddFile,
            file.CreatedByAccessId,
            string.Empty
        ));
        var changes = await dbContext.SaveChangesAsync();
        
        return changes <= 0 ? null : file;
    }

    public async Task<bool> AddCharEntryAsync(SupportfileCharacterEntryDbModel characterEntry, Ulid accessId)
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
        await supportfileLogsProxy.AddLogWithoutSaveAsync(dbContext, new(
            characterEntry.SupportfileId,
            SupportfileLogTypeEnum.AddCharEntry,
            accessId,
            $"Id: {characterEntry.Id} \n" +
            $"CharacterId: {characterEntry.CharacterId}"
        ));
        var changes = await dbContext.SaveChangesAsync();
        
        return changes > 0;
    }

    public async Task<bool> RemoveCharEntryAsync(SupportfileCharacterEntryDbModel characterEntry, Ulid accessId)
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
        await supportfileLogsProxy.AddLogWithoutSaveAsync(dbContext, new(
            characterEntry.SupportfileId,
            SupportfileLogTypeEnum.RemoveCharEntry,
            accessId,
            $"Id: {characterEntry.Id} \n" +
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
        
        var logType = file.Deleted ? SupportfileLogTypeEnum.DeleteFile : SupportfileLogTypeEnum.RestoreFile;
        
        dbContext.SupportfileDbModels.Update(file);
        await supportfileLogsProxy.AddLogWithoutSaveAsync(dbContext, new(
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
        await supportfileLogsProxy.AddLogWithoutSaveAsync(dbContext, new(
            file.Id,
            SupportfileLogTypeEnum.ModifyTitle,
            accessId,
            $"OldTitle: {oldTitle} \n" +
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
        await supportfileLogsProxy.AddLogWithoutSaveAsync(dbContext, new(
            file.Id,
            SupportfileLogTypeEnum.ModifyDescription,
            accessId,
            $"OldDescription: {oldDescription} \n" +
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
        await supportfileLogsProxy.AddLogWithoutSaveAsync(dbContext, new(
            file.Id,
            SupportfileLogTypeEnum.ModifyStatus,
            accessId,
            $"OldStatus: {oldStatus} \n" +
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
        await supportfileLogsProxy.AddLogWithoutSaveAsync(dbContext, new(
            file.Id,
            SupportfileLogTypeEnum.ModifyMinRank,
            accessId,
            $"OldMinRank: {oldMinRank} \n" +
            $"NewMinRank: {file.MinRank}"
        ));
        var changes = await dbContext.SaveChangesAsync();
        
        return changes > 0;
    }

    public async Task<bool> ChangeCategoryAsync(Ulid id, Ulid? newCategoryId, Ulid accessId)
    {
        var file = await GetAsync(id);
        if (file is null) return false;

        if (!newCategoryId.HasValue) return false;
        {
            var checkCategory = await supportfileCategoryProxy.GetAsync(newCategoryId.Value);
            if (checkCategory is null) return false;
        }
        
        await using var dbContext = await dbContextFactory.CreateDbContextAsync();
        
        var oldCategoryId = file.CategoryId; 
        file.CategoryId = newCategoryId;
        
        dbContext.SupportfileDbModels.Update(file);
        await supportfileLogsProxy.AddLogWithoutSaveAsync(dbContext, new(
            file.Id,
            SupportfileLogTypeEnum.ModifyCategory,
            accessId,
            $"OldCategoryId: {oldCategoryId} \n" +
                $"NewCategoryId: {file.CategoryId}"
        ));
        var changes = await dbContext.SaveChangesAsync();
        
        return changes > 0;
    }
}