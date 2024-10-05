using ChoiceVFileSystemBlazor.Database.Supportfiles.DbModels;

namespace ChoiceVFileSystemBlazor.Database.Supportfiles.Proxies.Interfaces;

public interface IFileLogsProxy
{
    public Task<List<FileLogsDbModel>> GetAllLogsForSupportfileIdAsync(Ulid id);

    // Return null if adding failed
    public Task<FileLogsDbModel?> AddLogAsync(FileLogsDbModel fileLog);

    public Task<bool> AddLogWithoutSaveAsync(ChoiceVFileSystemBlazorDatabaseContext dbContext, FileLogsDbModel fileLog);
}