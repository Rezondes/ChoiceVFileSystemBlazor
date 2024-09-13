using ChoiceVFileSystemBlazor.Database.Supportfiles.DbModels;

namespace ChoiceVFileSystemBlazor.Database.Supportfiles.Proxies.Interfaces;

public interface ISupportfileEntryProxy
{
    public Task<List<SupportfileEntryDbModel>> GetAllEntrysForSupportfileIdAsync(Ulid id);
    public Task<SupportfileEntryDbModel?> GetAsync(Ulid id);
    public Task<SupportfileEntryDbModel?> AddEntryAsync(SupportfileEntryDbModel supportfileEntry);
    public Task<bool> UpdateEntryContentAsync(Ulid id, string newContent, Ulid accessId);
    public Task<bool> RemoveEntryAsync(Ulid id, Ulid accessId);
    public Task<bool> RestoreEntryAsync(Ulid id, Ulid accessId);

    public Task<SupportfileFileUploadDbModel?> GetFileAsync(Ulid id);
    public int GetMaxFileSize();
    public Task<bool> RenameFileAsync(Ulid id, string newName, Ulid supportfileId, Ulid accessId);
    public Task<bool> AddFileAsync(SupportfileFileUploadDbModel file, Ulid supportfileId, Ulid accessId);
    public Task<bool> DeleteFileAsync(Ulid id, Ulid supportfileId, Ulid accessId);
}