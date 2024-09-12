using ChoiceVFileSystemBlazor.Database._Shared;
using ChoiceVFileSystemBlazor.Database.Accesses.DbModels;
using ChoiceVFileSystemBlazor.Database.Accesses.DbModels.Partials;
using ChoiceVFileSystemBlazor.Database.Accesses.Enums;
using ChoiceVFileSystemBlazor.Database.Accesses.Proxies.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace ChoiceVFileSystemBlazor.Database.Accesses.Proxies;

public class AccessProxy(IDbContextFactory<ChoiceVFileSystemBlazorDatabaseContext> dbContextFactory, IAccessLogsProxy accessLogsProxy) : IAccessProxy
{
    public async Task<List<AccessDbModel>> GetAllAsync()
    {
        await using var dbContext = await dbContextFactory.CreateDbContextAsync();
        
        return await dbContext.AccessDbModels.AsNoTracking().ToListAsync();
    }

    public async Task<AccessDbModel?> GetAsync(string discordId)
    {
        await using var dbContext = await dbContextFactory.CreateDbContextAsync();

        return dbContext.AccessDbModels.AsNoTracking().FirstOrDefault(x => x.DiscordId == discordId);
    }
    
    public async Task<AccessDbModel?> GetAsync(int accountId)
    {
        await using var dbContext = await dbContextFactory.CreateDbContextAsync();

        return dbContext.AccessDbModels.AsNoTracking().FirstOrDefault(x => x.AccountId == accountId);
    }

    public async Task<AccessDbModel?> GetAsync(Ulid id)
    {
        await using var dbContext = await dbContextFactory.CreateDbContextAsync();

        return dbContext.AccessDbModels.AsNoTracking().FirstOrDefault(x => x.Id == id);
    }
    
    public async Task<bool> AddAccessModelAsync(AccessDbModel accessModel)
    {
        var checkDiscordId = await GetAsync(accessModel.DiscordId);
        if (checkDiscordId is null) return false;
        var checkAccountId = await GetAsync(accessModel.AccountId);
        if (checkAccountId is null) return false;
        
        await using var dbContext = await dbContextFactory.CreateDbContextAsync();
        
        await dbContext.AccessDbModels.AddAsync(accessModel);
        await accessLogsProxy.AddLogWithoutSaveAsync(dbContext, new(
            accessModel.Id,
            AccessLogTypeEnum.ModifyAccountId,
            Ulid.Empty,
            string.Empty
        ));
        var changes = await dbContext.SaveChangesAsync();
        
        return changes > 0;
    }

    public async Task<bool> UpdateNameAsync(Ulid id, string newName, Ulid accessId)
    {
        var accessDbModel = await GetAsync(id);
        if (accessDbModel is null) return false;
        
        await using var dbContext = await dbContextFactory.CreateDbContextAsync();

        var oldName = accessDbModel.Name;
        
        accessDbModel.Name = newName;
        
        dbContext.AccessDbModels.Update(accessDbModel);
        await accessLogsProxy.AddLogWithoutSaveAsync(dbContext, new(
            accessDbModel.Id,
            AccessLogTypeEnum.ModifyName,
            accessId,
            $"OldName: {oldName} \n" +
            $"NewName: {accessDbModel.Name}"
        ));
        var changes = await dbContext.SaveChangesAsync();
        
        return changes > 0;
    }
    
    public async Task<bool> UpdateAccountIdAsync(Ulid id, int accountId, Ulid accessId)
    {
        var accessDbModel = await GetAsync(id);
        if (accessDbModel is null) return false;
        
        await using var dbContext = await dbContextFactory.CreateDbContextAsync();

        var oldAccountId = accessDbModel.AccountId;
        
        accessDbModel.AccountId = accountId;
        
        dbContext.AccessDbModels.Update(accessDbModel);
        await accessLogsProxy.AddLogWithoutSaveAsync(dbContext, new(
            accessDbModel.Id,
            AccessLogTypeEnum.ModifyAccountId,
            accessId,
            $"OldAccountId: {oldAccountId} \n" +
            $"NewAccountId: {accessDbModel.AccountId}"
        ));
        var changes = await dbContext.SaveChangesAsync();
        
        return changes > 0;
    }
    
    public async Task<bool> UpdateDiscordIdAsync(Ulid id, string newDiscordId, Ulid accessId)
    {
        var accessDbModel = await GetAsync(id);
        if (accessDbModel is null) return false;
        
        await using var dbContext = await dbContextFactory.CreateDbContextAsync();

        var oldDiscordId = accessDbModel.DiscordId;
        
        accessDbModel.DiscordId = newDiscordId;
        
        dbContext.AccessDbModels.Update(accessDbModel);
        await accessLogsProxy.AddLogWithoutSaveAsync(dbContext, new(
            accessDbModel.Id,
            AccessLogTypeEnum.ModifyDiscordId,
            accessId,
            $"OldDiscordId: {oldDiscordId} \n" +
            $"NewDiscordId: {accessDbModel.DiscordId}"
        ));
        var changes = await dbContext.SaveChangesAsync();
        
        return changes > 0;
    }
    
    public async Task<bool> UpdateRankAsync(Ulid id, RankEnum newRank, Ulid accessId)
    {
        var accessDbModel = await GetAsync(id);
        if (accessDbModel is null) return false;
        
        await using var dbContext = await dbContextFactory.CreateDbContextAsync();

        var oldRank = accessDbModel.Rank;
        
        accessDbModel.Rank = newRank;
        
        dbContext.AccessDbModels.Update(accessDbModel);
        await accessLogsProxy.AddLogWithoutSaveAsync(dbContext, new(
            accessDbModel.Id,
            AccessLogTypeEnum.ModifyRank,
            accessId,
            $"OldRank: {oldRank} \n" +
            $"NewRank: {accessDbModel.Rank}"
        ));
        var changes = await dbContext.SaveChangesAsync();
        
        return changes > 0;
    }

    public async Task<bool> UpdateToPartial(
        AccessDbModel accessDbModel,
        PartialAccessModel partialAccessModel,
        Ulid accessId,
        bool updateName = true,
        bool updateAccountId = true,
        bool updateDiscordId = true,
        bool updateRank = true
    )
    {
        await using var dbContext = await dbContextFactory.CreateDbContextAsync();

        if (updateName && accessDbModel.Name != partialAccessModel.Name)
        {
            var oldName = accessDbModel.Name;
            accessDbModel.Name = partialAccessModel.Name;
            await accessLogsProxy.AddLogWithoutSaveAsync(dbContext, new(
                accessDbModel.Id,
                AccessLogTypeEnum.ModifyName,
                accessId,
                $"OldName: {oldName} \n" +
                $"NewName: {accessDbModel.Name}"
            ));
        }
        if (updateAccountId && accessDbModel.AccountId != partialAccessModel.AccountId)
        {
            var oldAccountId = accessDbModel.AccountId;
            accessDbModel.AccountId = partialAccessModel.AccountId;
            await accessLogsProxy.AddLogWithoutSaveAsync(dbContext, new(
                accessDbModel.Id,
                AccessLogTypeEnum.ModifyAccountId,
                accessId,
                $"OldAccountId: {oldAccountId} \n" +
                $"NewAccountId: {accessDbModel.AccountId}"
            ));
        }
        if (updateDiscordId && accessDbModel.DiscordId != partialAccessModel.DiscordId)
        {
            var oldDiscordId = accessDbModel.DiscordId;
            accessDbModel.DiscordId = partialAccessModel.DiscordId;
            await accessLogsProxy.AddLogWithoutSaveAsync(dbContext, new(
                accessDbModel.Id,
                AccessLogTypeEnum.ModifyDiscordId,
                accessId,
                $"OldDiscordId: {oldDiscordId} \n" +
                $"NewDiscordId: {accessDbModel.DiscordId}"
            ));
        }
        if (updateRank && accessDbModel.Rank != partialAccessModel.Rank)
        {
            var oldRank = accessDbModel.Rank;
            accessDbModel.Rank = partialAccessModel.Rank;
            await accessLogsProxy.AddLogWithoutSaveAsync(dbContext, new(
                accessDbModel.Id,
                AccessLogTypeEnum.ModifyRank,
                accessId,
                $"OldRank: {oldRank} \n" +
                $"NewRank: {accessDbModel.Rank}"
            ));
        }
        
        dbContext.AccessDbModels.Update(accessDbModel);
        
        var changes = await dbContext.SaveChangesAsync();
        
        return changes > 0;
    }
}