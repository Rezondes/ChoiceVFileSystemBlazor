using ChoiceVFileSystemBlazor.Database.Supportfiles.DbModels;

namespace ChoiceVFileSystemBlazor.Database.Supportfiles.Proxies.Interfaces;

public interface IFileEntryProxy
{
    public Task<List<FileEntryDbModel>> GetAllEntrysForSupportfileIdAsync(Ulid id);
    public Task<FileEntryDbModel?> GetAsync(Ulid id);
    public Task<FileEntryDbModel?> AddEntryAsync(FileEntryDbModel fileEntry);
    public Task<bool> UpdateEntryContentAsync(Ulid id, string newContent, Ulid accessId);
    public Task<bool> RemoveEntryAsync(Ulid id, Ulid accessId);
    public Task<bool> RestoreEntryAsync(Ulid id, Ulid accessId);

    public Task<FileUploadDbModel?> GetFileAsync(Ulid id);
    public long GetMaxFileSize();
    public Task<bool> RenameFileAsync(Ulid id, string newName, Ulid supportfileId, Ulid accessId);
    public Task<bool> AddFileAsync(FileUploadDbModel file, Ulid supportfileId, Ulid accessId);
    public Task<bool> DeleteFileAsync(Ulid id, Ulid supportfileId, Ulid accessId);
}