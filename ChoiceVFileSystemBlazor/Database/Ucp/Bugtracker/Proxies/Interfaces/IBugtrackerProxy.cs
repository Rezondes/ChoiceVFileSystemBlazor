using ChoiceVFileSystemBlazor.Database.Ucp.Bugtracker.DbModels;
using ChoiceVFileSystemBlazor.Database.Ucp.Bugtracker.Model;

namespace ChoiceVFileSystemBlazor.Database.Ucp.Bugtracker.Proxies.Interfaces;

public interface IBugtrackerProxy
{
    public Task<List<DiscordIdToBugTaskIdDbModel>> GetAllAsync();
    public Task<List<BugTaskModel>> GetAllBugsAsync(CancellationToken cancellationToken = default);
    public Task<List<DiscordIdToBugTaskIdDbModel>> GetAllForDiscordIdAsync(string discordId);
    public Task<DiscordIdToBugTaskIdDbModel> AddAsync(DiscordIdToBugTaskIdDbModel model);
    public Task RemoveAsync(Ulid id);
}