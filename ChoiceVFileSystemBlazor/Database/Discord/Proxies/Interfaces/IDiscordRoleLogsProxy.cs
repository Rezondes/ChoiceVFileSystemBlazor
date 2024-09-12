using ChoiceVFileSystemBlazor.Database.Discord.DbModels;

namespace ChoiceVFileSystemBlazor.Database.Discord.Proxies.Interfaces;

public interface IDiscordRoleLogsProxy
{
    public Task<List<DiscordRoleLogsDbModel>> GetAllAsync();

    public Task<DiscordRoleLogsDbModel?> AddLogAsync(DiscordRoleLogsDbModel logFile);

    public Task<bool> AddLogWithoutSaveAsync(ChoiceVFileSystemBlazorDatabaseContext dbContext, DiscordRoleLogsDbModel logFile);
}