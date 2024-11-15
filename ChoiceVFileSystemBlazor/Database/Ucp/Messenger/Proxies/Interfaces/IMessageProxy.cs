using ChoiceVFileSystemBlazor.Database.Ucp.Bugtracker.DbModels;
using ChoiceVFileSystemBlazor.Database.Ucp.Messenger.DbModels;
using ChoiceVFileSystemBlazor.Database.Ucp.Messenger.Models;

namespace ChoiceVFileSystemBlazor.Database.Ucp.Messenger.Proxies.Interfaces;

public interface IMessageProxy
{
    public Task<List<MessageToDiscordIdDbModel>> GetAllAsync(CancellationToken cancellationToken = default);
    public Task<List<ChatForDiscordIdModel>> GetAllChatsAsync(CancellationToken cancellationToken = default);
    public Task<List<MessageToDiscordIdDbModel>> GetAllForDiscordIdAsync(string discordId, CancellationToken cancellationToken = default);
    public Task<MessageToDiscordIdDbModel> AddAsync(MessageToDiscordIdDbModel model);
    public Task RemoveAsync(Ulid id);
}