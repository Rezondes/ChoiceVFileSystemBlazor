using ChoiceVFileSystemBlazor.Database._Shared;
using ChoiceVFileSystemBlazor.Database.Accesses.DbModels;
using ChoiceVFileSystemBlazor.Database.Accesses.DbModels.Partials;

namespace ChoiceVFileSystemBlazor.Database.Accesses.Proxies.Interfaces;

public interface IAccessProxy
{
    public Task<List<AccessDbModel>> GetAllAsync();
    public Task<AccessDbModel?> GetAsync(string discordId);
    public Task<AccessDbModel?> GetWithSettingsAsync(string discordId);
    public Task<AccessDbModel?> GetAsync(Ulid id);
    public Task<AccessDbModel?> GetWithSettingsAsync(Ulid id);
    public Task<AccessDbModel?> GetFullAsync(Ulid id);
    public Task<bool> AddAccessModelAsync(AccessDbModel accessModel);
    public Task<bool> UpdateNameAsync(Ulid id, string newName, Ulid accessId);
    public Task<bool> UpdateAccountIdAsync(Ulid id, int accountId, Ulid accessId);
    public Task<bool> UpdateDiscordIdAsync(Ulid id, string newDiscordId, Ulid accessId);
    public Task<bool> UpdateRankAsync(Ulid id, RankEnum newRank, Ulid accessId);
    public Task<bool> UpdateToPartial(
        AccessDbModel accessDbModel, 
        PartialAccessModel partialAccessModel,
        Ulid accessId,
        bool updateName = true,
        bool updateAccountId = true,
        bool updateDiscordId = true,
        bool updateRank = true
    );

    public Task<AccessSettingsDbModel?> AddSettingsAsync(AccessDbModel accessDbModel);
}