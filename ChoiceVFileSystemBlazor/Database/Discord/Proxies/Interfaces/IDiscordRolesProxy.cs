using ChoiceVFileSystemBlazor.Database.Discord.DbModels;
using ChoiceVFileSystemBlazor.Database.Discord.DbModels.Partials;
using ChoiceVFileSystemBlazor.Database.Discord.Enums;

namespace ChoiceVFileSystemBlazor.Database.Discord.Proxies.Interfaces;

public interface IDiscordRolesProxy
{
    public Task<List<DiscordRoleDbModel>> GetAllAsync();

    public Task<DiscordRoleDbModel?> GetByIdAsync(Ulid id);
    
    public Task<bool> NameExistsAsync(string name);
    
    public Task<bool> DiscordRoleIdExistsAsync(ulong discordRoleId);
    public Task<bool> AddAsync(DiscordRoleDbModel role, Ulid accessId);
    
    public Task<bool> RemoveAsync(Ulid roleId, Ulid accessId);

    public Task<bool> UpdateToPartial(
        DiscordRoleDbModel model,
        PartialDiscordRoleDbModel partialModel,
        Ulid accessId,
        bool updateName = true,
        bool updateDiscordRoleId = true,
        bool updateAutomaticRank = true
    );
}