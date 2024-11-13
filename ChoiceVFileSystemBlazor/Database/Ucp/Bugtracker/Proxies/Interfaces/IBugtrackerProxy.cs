using ChoiceVFileSystemBlazor.Database.Ucp.Bugtracker.DbModels;

namespace ChoiceVFileSystemBlazor.Database.Ucp.Bugtracker.Proxies.Interfaces;

public interface IBugtrackerProxy
{
    public Task<List<DiscordIdToBugTaskIdDbModel>> GetAllAsync();
    public Task<List<DiscordIdToBugTaskIdDbModel>> GetAllForDiscordIdAsync(string discordId);
    public Task<DiscordIdToBugTaskIdDbModel> AddAsync(DiscordIdToBugTaskIdDbModel model);
    public Task RemoveAsync(Ulid id);
}