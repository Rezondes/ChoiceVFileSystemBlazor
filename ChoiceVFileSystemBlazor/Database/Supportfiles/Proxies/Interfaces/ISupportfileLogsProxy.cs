using ChoiceVFileSystemBlazor.Database.Supportfiles.DbModels;

namespace ChoiceVFileSystemBlazor.Database.Supportfiles.Proxies.Interfaces;

public interface ISupportfileLogsProxy
{
    public Task<List<SupportfileLogsDbModel>> GetAllLogsForSupportfileIdAsync(Ulid id);

    // Return null if adding failed
    public Task<SupportfileLogsDbModel?> AddLogAsync(SupportfileLogsDbModel supportfileLog);

    public Task<bool> AddLogWithoutSaveAsync(ChoiceVFileSystemBlazorDatabaseContext dbContext, SupportfileLogsDbModel supportfileLog);
}