using ChoiceVFileSystemBlazor.Database._Shared;
using ChoiceVFileSystemBlazor.Database.Supportfiles.DbModels;

namespace ChoiceVFileSystemBlazor.Database.Supportfiles.Proxies.Interfaces;

public interface IFileProxy
{
    public Task<List<FileDbModel>> GetAllSupportfilesAsync();

    public Task<List<FileDbModel>> GetAllFullSupportfilesAsync();
    
    public Task<FileDbModel?> GetAsync(Ulid id);
    
    public Task<FileDbModel?> GetFullAsync(Ulid id);
    
    public Task<FileDbModel?> AddAsync(FileDbModel file);

    public Task<bool> AddCharEntryAsync(FileCharacterEntryDbModel characterEntry, Ulid accessId);

    public Task<bool> RemoveCharEntryAsync(FileCharacterEntryDbModel characterEntry, Ulid accessId);

    public Task<bool> ToggleDeletedAsync(Ulid id, Ulid accessId);

    public Task<bool> UpdateTitleAsync(Ulid id, string newTitle, Ulid accessId);

    public Task<bool> UpdateDescriptionAsync(Ulid id, string newDescription, Ulid accessId);

    public Task<bool> UpdateStatusAsync(Ulid id, FileStatusEnum newStatus, Ulid accessId);

    public Task<bool> UpdateMinRankAsync(Ulid id, RankEnum newMinRank, Ulid accessId);
    
    public Task<bool> ChangeCategoryAsync(Ulid id, Ulid? newCategoryId, Ulid accessId);
}