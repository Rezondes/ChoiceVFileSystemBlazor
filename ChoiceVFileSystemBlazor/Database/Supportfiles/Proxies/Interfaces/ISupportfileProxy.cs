using ChoiceVFileSystemBlazor.Database._Shared;
using ChoiceVFileSystemBlazor.Database.Supportfiles.DbModels;

namespace ChoiceVFileSystemBlazor.Database.Supportfiles.Proxies.Interfaces;

public interface ISupportfileProxy
{
    public Task<List<SupportfileDbModel>> GetAllAsync();

    public Task<List<SupportfileDbModel>> GetAllFullAsync();
    
    public Task<SupportfileDbModel?> GetAsync(Ulid id);
    
    public Task<SupportfileDbModel?> GetFullAsync(Ulid id);
    
    public Task<SupportfileDbModel?> AddAsync(SupportfileDbModel file);

    public Task<bool> AddCharEntryAsync(SupportfileCharacterEntryDbModel characterEntry, Ulid accessId);

    public Task<bool> RemoveCharEntryAsync(SupportfileCharacterEntryDbModel characterEntry, Ulid accessId);

    public Task<bool> ToggleDeletedAsync(Ulid id, Ulid accessId);

    public Task<bool> UpdateTitleAsync(Ulid id, string newTitle, Ulid accessId);

    public Task<bool> UpdateDescriptionAsync(Ulid id, string newDescription, Ulid accessId);

    public Task<bool> UpdateStatusAsync(Ulid id, FileStatusEnum newStatus, Ulid accessId);

    public Task<bool> UpdateMinRankAsync(Ulid id, RankEnum newMinRank, Ulid accessId);
}