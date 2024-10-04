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

    private readonly Ulid _systemAccessId = Ulid.Empty;
    public async Task<bool> AddSystemAccessAsync()
    {
        using var dbContext = await CreateDbContextAsync();
        
        var serverAccess = await GetAsync(Ulid.Empty);
        if (serverAccess is not null) return false;
        
        var systemAccess = new AccessDbModel(0, "0", "System", RankEnum.None);
        systemAccess.Id = _systemAccessId;
            
        var response = await AddAccessModelAsync(systemAccess);
        return response;
    }
    
    private readonly SemaphoreSlim _semaphore = new SemaphoreSlim(1, 1);
    
    public async Task<bool> AddAccessModelAsync(AccessDbModel accessModel)
    {
        try
        {
            await _semaphore.WaitAsync();
            try
            {
                using var dbContext = await CreateDbContextAsync();
                var checkDiscordId = await GetAsync(accessModel.DiscordId);
                if (checkDiscordId is not null) return false;
                var checkAccountId = await GetAsync(accessModel.AccountId);
                if (checkAccountId is not null) return false;

                await dbContext.AccessSettingsDbModels.AddAsync(new AccessSettingsDbModel(accessModel.Id));
                await dbContext.AccessDbModels.AddAsync(accessModel);
                var changes = await dbContext.SaveChangesAsync();

                return changes > 0;
            }
            finally
            {
                _semaphore.Release();
            }
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return false;
        }
    }

    public async Task<bool> UpdateNameAsync(Ulid id, string newName, Ulid accessId)
    {
        if (IsIdEqualSystemAccessId(id)) return false;
        
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
            $"OldName: {oldName} \n\n" +
            $"NewName: {accessDbModel.Name}"
        ));
        var changes = await dbContext.SaveChangesAsync();

        return changes > 0;
    }

    private bool IsIdEqualSystemAccessId(Ulid id)
    {
        return id == _systemAccessId;
    }

    public async Task<bool> UpdateAccountIdAsync(Ulid id, int accountId, Ulid accessId)
    {
        if (IsIdEqualSystemAccessId(id)) return false;
        
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
            $"OldAccountId: {oldAccountId} \n\n" +
            $"NewAccountId: {accessDbModel.AccountId}"
        ));
        var changes = await dbContext.SaveChangesAsync();

        return changes > 0;
    }

    public async Task<bool> UpdateDiscordIdAsync(Ulid id, string newDiscordId, Ulid accessId)
    {
        if (IsIdEqualSystemAccessId(id)) return false;

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
            $"OldDiscordId: {oldDiscordId} \n\n" +
            $"NewDiscordId: {accessDbModel.DiscordId}"
        ));
        var changes = await dbContext.SaveChangesAsync();

        return changes > 0;
    }

    public async Task<bool> UpdateRankAsync(Ulid id, RankEnum newRank, Ulid accessId)
    {
        if (IsIdEqualSystemAccessId(id)) return false;

        using var dbContext = await CreateDbContextAsync();
        var accessDbModel = await GetAsync(id);
        if (accessDbModel is null) return false;

        var oldRank = accessDbModel.Rank;
        accessDbModel.Rank = newRank;

        dbContext.AccessDbModels.Update(accessDbModel);
        await _accessLogsProxy.AddLogWithoutSaveAsync(dbContext, new(
            accessDbModel.Id,
            AccessLogTypeEnum.ModifyRank,
            accessId,
            $"OldRank: {oldRank} \n\n" +
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
        if (IsIdEqualSystemAccessId(accessDbModel.Id)) return false;

        using var dbContext = await CreateDbContextAsync();
        if (updateName && accessDbModel.Name != partialAccessModel.Name)
        {
            var oldName = accessDbModel.Name;
            accessDbModel.Name = partialAccessModel.Name;
            await _accessLogsProxy.AddLogWithoutSaveAsync(dbContext, new(
                accessDbModel.Id,
                AccessLogTypeEnum.ModifyName,
                accessId,
                $"OldName: {oldName} \n\n" +
                $"NewName: {accessDbModel.Name}"
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
                $"OldAccountId: {oldAccountId} \n\n" +
                $"NewAccountId: {accessDbModel.AccountId}"
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
                $"OldDiscordId: {oldDiscordId} \n\n" +
                $"NewDiscordId: {accessDbModel.DiscordId}"
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
                $"OldRank: {oldRank} \n\n" +
                $"NewRank: {accessDbModel.Rank}"
            ));
        }

        dbContext.AccessDbModels.Update(accessDbModel);

        var changes = await dbContext.SaveChangesAsync();

        return changes > 0;
    }

    public async Task<bool> UpdateIsDarkModeAsync(Ulid id, bool isDarkMode, Ulid accessId)
    {
        if (IsIdEqualSystemAccessId(id)) return false;
        
        var check = await GetWithSettingsAsync(id);
        if (check is null) return false;
        
        using var dbContext = await CreateDbContextAsync();

        if (check.Settings.IsDarkMode == isDarkMode) return false;

        var oldIsDarkMode = check.Settings.IsDarkMode;
        check.Settings.IsDarkMode = isDarkMode;
        
        dbContext.AccessSettingsDbModels.Update(check.Settings);
        await _accessLogsProxy.AddLogWithoutSaveAsync(dbContext, new(
            check.Id,
            AccessLogTypeEnum.ModifyIsDarkMode,
            accessId,
            $"OldIsDarkMode: {oldIsDarkMode} \n\n" +
            $"NewIsDarkMode: {check.Settings.IsDarkMode}"
        ));
        var changes = await dbContext.SaveChangesAsync();

        return changes > 0;
    }
    
    public async Task<bool> UpdateIsNavbarExpandedAsync(Ulid id, bool isNavbarExpanded, Ulid accessId)
    {
        if (IsIdEqualSystemAccessId(id)) return false;
        
        var check = await GetWithSettingsAsync(id);
        if (check is null) return false;
        
        using var dbContext = await CreateDbContextAsync();

        if (check.Settings.IsNavbarExpanded == isNavbarExpanded) return false;

        var oldIsNavbarExpanded = check.Settings.IsNavbarExpanded;
        check.Settings.IsNavbarExpanded = isNavbarExpanded;
        
        dbContext.AccessSettingsDbModels.Update(check.Settings);
        await _accessLogsProxy.AddLogWithoutSaveAsync(dbContext, new(
            check.Id,
            AccessLogTypeEnum.ModifyIsNavbarExpanded,
            accessId,
            $"OldIsNavbarExpanded: {oldIsNavbarExpanded} \n\n" +
            $"NewIsNavbarExpanded: {check.Settings.IsNavbarExpanded}"
        ));
        var changes = await dbContext.SaveChangesAsync();

        return changes > 0;
    }
    
    public async Task<bool> UpdateTimeZoneAsync(Ulid id, string timeZone, Ulid accessId)
    {
        if (IsIdEqualSystemAccessId(id)) return false;

        var check = await GetWithSettingsAsync(id);
        if (check is null) return false;
        
        using var dbContext = await CreateDbContextAsync();

        if (check.Settings.TimeZone == timeZone) return false;

        var oldTimeZone = check.Settings.TimeZone;
        check.Settings.TimeZone = timeZone;
        
        dbContext.AccessSettingsDbModels.Update(check.Settings);
        await _accessLogsProxy.AddLogWithoutSaveAsync(dbContext, new(
            check.Id,
            AccessLogTypeEnum.ModifyTimezone,
            accessId,
            $"OldTimezone: {oldTimeZone} \n\n" +
            $"NewTimezone: {check.Settings.TimeZone}"
        ));
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