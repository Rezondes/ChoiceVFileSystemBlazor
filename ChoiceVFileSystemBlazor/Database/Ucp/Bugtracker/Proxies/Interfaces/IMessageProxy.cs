using ChoiceVFileSystemBlazor.Database.Ucp.Bugtracker.DbModels;

namespace ChoiceVFileSystemBlazor.Database.Ucp.Bugtracker.Proxies.Interfaces;

public interface IMessageProxy
{
    public Task<List<MessageToDiscordIdDbModel>> GetAllAsync();
    public Task<List<MessageToDiscordIdDbModel>> GetAllForDiscordIdAsync(string discordId);
    public Task<MessageToDiscordIdDbModel> AddAsync(MessageToDiscordIdDbModel model);
    public Task RemoveAsync(Ulid id);
}