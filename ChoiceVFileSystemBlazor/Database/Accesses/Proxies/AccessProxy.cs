using ChoiceVFileSystemBlazor.Database._Shared;
using ChoiceVFileSystemBlazor.Database.Accesses.DbModels;
using ChoiceVFileSystemBlazor.Database.Accesses.DbModels.Partials;
using ChoiceVFileSystemBlazor.Database.Accesses.Enums;
using ChoiceVFileSystemBlazor.Database.Accesses.Proxies.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace ChoiceVFileSystemBlazor.Database.Accesses.Proxies;

public class AccessProxy : IAccessProxy
{
    private readonly IDbContextFactory<ChoiceVFileSystemBlazorDatabaseContext> _dbContextFactory;
    private readonly IAccessLogsProxy _accessLogsProxy;

    public AccessProxy(IDbContextFactory<ChoiceVFileSystemBlazorDatabaseContext> dbContextFactory, IAccessLogsProxy accessLogsProxy)
    {
        _dbContextFactory = dbContextFactory;
        _accessLogsProxy = accessLogsProxy;
    }

    private async Task<ChoiceVFileSystemBlazorDatabaseContext> CreateDbContextAsync()
    {
        return await _dbContextFactory.CreateDbContextAsync();
    }

    public async Task<List<AccessDbModel>> GetAllAsync()
    {
        using var dbContext = await CreateDbContextAsync();
        return await dbContext.AccessDbModels.AsNoTracking().ToListAsync();
    }

    public async Task<AccessDbModel?> GetAsync(string discordId)
    {
        using var dbContext = await CreateDbContextAsync();
        return await dbContext.AccessDbModels
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.DiscordId == discordId);
    }

    public async Task<AccessDbModel?> GetWithSettingsAsync(string discordId)
    {
        using var dbContext = await CreateDbContextAsync();
        return await dbContext.AccessDbModels
            .AsNoTracking()
            .Include(x => x.Settings)
            .FirstOrDefaultAsync(x => x.DiscordId == discordId);
    }
    
    public async Task<AccessDbModel?> GetAsync(int accountId)
    {
        using var dbContext = await CreateDbContextAsync();
        return await dbContext.AccessDbModels
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.AccountId == accountId);
    }

    public async Task<AccessDbModel?> GetAsync(Ulid id)
    {
        using var dbContext = await CreateDbContextAsync();
        return await dbContext.AccessDbModels
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Id == id);
    }

    public async Task<AccessDbModel?> GetWithSettingsAsync(Ulid id)
    {
        using var dbContext = await CreateDbContextAsync();
        return await dbContext.AccessDbModels
            .AsNoTracking()
            .Include(x => x.Settings)
            .FirstOrDefaultAsync(x => x.Id == id);
    }
    
    public async Task<AccessDbModel?> GetFullAsync(Ulid id)
    {
        using var dbContext = await CreateDbContextAsync();
        return await dbContext.AccessDbModels
            .AsNoTracking()
            .Include(x => x.Settings)
            .Include(x => x.Supportfiles)
            .Include(x => x.SupportfileLogs)
            .Include(x => x.SupportfileEntrys)
            .Include(x => x.CreatedAccessLogs)
            .Include(x => x.TargetedAccessLogs)
            .FirstOrDefaultAsync(x => x.Id == id);
    }
    
    public async Task<bool> AddAccessModelAsync(AccessDbModel accessModel)
    {
        try
        {
            using var dbContext = await CreateDbContextAsync();
            var checkDiscordId = await GetAsync(accessModel.DiscordId);
            if (checkDiscordId is not null) return false;
            // var checkAccountId = await GetAsync(accessModel.AccountId);
            // if (checkAccountId is not null) return false;

            await dbContext.AccessSettingsDbModels.AddAsync(new AccessSettingsDbModel(accessModel.Id));
            await dbContext.AccessDbModels.AddAsync(accessModel);
            var changes = await dbContext.SaveChangesAsync();

            return changes > 0;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return false;
        }
    }

    public async Task<bool> UpdateNameAsync(Ulid id, string newName, Ulid accessId)
    {
        using var dbContext = await CreateDbContextAsync();
        var accessDbModel = await GetAsync(id);
        if (accessDbModel is null) return false;

        var oldName = accessDbModel.Name;
        accessDbModel.Name = newName;

        dbContext.AccessDbModels.Update(accessDbModel);
        await _accessLogsProxy.AddLogWithoutSaveAsync(dbContext, new(
            accessDbModel.Id,
            AccessLogTypeEnum.ModifyName,
            accessId,
            $"OldName: {oldName} \nNewName: {accessDbModel.Name}"
        ));
        var changes = await dbContext.SaveChangesAsync();

        return changes > 0;
    }

    public async Task<bool> UpdateAccountIdAsync(Ulid id, int accountId, Ulid accessId)
    {
        using var dbContext = await CreateDbContextAsync();
        var accessDbModel = await GetAsync(id);
        if (accessDbModel is null) return false;

        var oldAccountId = accessDbModel.AccountId;
        accessDbModel.AccountId = accountId;

        dbContext.AccessDbModels.Update(accessDbModel);
        await _accessLogsProxy.AddLogWithoutSaveAsync(dbContext, new(
            accessDbModel.Id,
            AccessLogTypeEnum.ModifyAccountId,
            accessId,
            $"OldAccountId: {oldAccountId} \nNewAccountId: {accessDbModel.AccountId}"
        ));
        var changes = await dbContext.SaveChangesAsync();

        return changes > 0;
    }

    public async Task<bool> UpdateDiscordIdAsync(Ulid id, string newDiscordId, Ulid accessId)
    {
        using var dbContext = await CreateDbContextAsync();
        var accessDbModel = await GetAsync(id);
        if (accessDbModel is null) return false;

        var oldDiscordId = accessDbModel.DiscordId;
        accessDbModel.DiscordId = newDiscordId;

        dbContext.AccessDbModels.Update(accessDbModel);
        await _accessLogsProxy.AddLogWithoutSaveAsync(dbContext, new(
            accessDbModel.Id,
            AccessLogTypeEnum.ModifyDiscordId,
            accessId,
            $"OldDiscordId: {oldDiscordId} \nNewDiscordId: {accessDbModel.DiscordId}"
        ));
        var changes = await dbContext.SaveChangesAsync();

        return changes > 0;
    }

    public async Task<bool> UpdateRankAsync(Ulid id, RankEnum newRank, Ulid accessId)
    {
        using var dbContext = await CreateDbContextAsync();
        var accessDbModel = await GetAsync(id);
        if (accessDbModel is null) return false;

        var oldRank = accessDbModel.Rank;
        accessDbModel.Rank = newRank;

        dbContext.AccessDbModels.Update(accessDbModel);
        if (accessId != Ulid.Empty)
        {
            await _accessLogsProxy.AddLogWithoutSaveAsync(dbContext, new(
                accessDbModel.Id,
                AccessLogTypeEnum.ModifyRank,
                accessId,
                $"OldRank: {oldRank} \nNewRank: {accessDbModel.Rank}"
            ));
        }
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
        using var dbContext = await CreateDbContextAsync();
        if (updateName && accessDbModel.Name != partialAccessModel.Name)
        {
            var oldName = accessDbModel.Name;
            accessDbModel.Name = partialAccessModel.Name;
            await _accessLogsProxy.AddLogWithoutSaveAsync(dbContext, new(
                accessDbModel.Id,
                AccessLogTypeEnum.ModifyName,
                accessId,
                $"OldName: {oldName} \nNewName: {accessDbModel.Name}"
            ));
        }
        if (updateAccountId && accessDbModel.AccountId != partialAccessModel.AccountId)
        {
            var oldAccountId = accessDbModel.AccountId;
            accessDbModel.AccountId = partialAccessModel.AccountId;
            await _accessLogsProxy.AddLogWithoutSaveAsync(dbContext, new(
                accessDbModel.Id,
                AccessLogTypeEnum.ModifyAccountId,
                accessId,
                $"OldAccountId: {oldAccountId} \nNewAccountId: {accessDbModel.AccountId}"
            ));
        }
        if (updateDiscordId && accessDbModel.DiscordId != partialAccessModel.DiscordId)
        {
            var oldDiscordId = accessDbModel.DiscordId;
            accessDbModel.DiscordId = partialAccessModel.DiscordId;
            await _accessLogsProxy.AddLogWithoutSaveAsync(dbContext, new(
                accessDbModel.Id,
                AccessLogTypeEnum.ModifyDiscordId,
                accessId,
                $"OldDiscordId: {oldDiscordId} \nNewDiscordId: {accessDbModel.DiscordId}"
            ));
        }
        if (updateRank && accessDbModel.Rank != partialAccessModel.Rank)
        {
            var oldRank = accessDbModel.Rank;
            accessDbModel.Rank = partialAccessModel.Rank;
            await _accessLogsProxy.AddLogWithoutSaveAsync(dbContext, new(
                accessDbModel.Id,
                AccessLogTypeEnum.ModifyRank,
                accessId,
                $"OldRank: {oldRank} \nNewRank: {accessDbModel.Rank}"
            ));
        }

        dbContext.AccessDbModels.Update(accessDbModel);

        var changes = await dbContext.SaveChangesAsync();

        return changes > 0;
    }

    public async Task<AccessSettingsDbModel?> AddSettingsAsync(AccessDbModel accessDbModel)
    {
        var check = await GetAsync(accessDbModel.Id);
        if (check is null) return null;
        
        using var dbContext = await CreateDbContextAsync();

        var settings = new AccessSettingsDbModel(accessDbModel.Id);
        
        await dbContext.AccessSettingsDbModels.AddAsync(settings);
        var changes = await dbContext.SaveChangesAsync();

        return changes > 0 ? settings : null;
    }
}