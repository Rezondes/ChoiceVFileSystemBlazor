using ChoiceVFileSystemBlazor.Database.Accesses.DbModels;

namespace ChoiceVFileSystemBlazor.Database.Accesses.Proxies.Interfaces;

public interface IAccessLogsProxy
{
    public Task<List<AccessLogsDbModel>> GetAllAsync();
    public Task<AccessLogsDbModel?> AddLogAsync(AccessLogsDbModel accessLog);

    public Task<bool> AddLogWithoutSaveAsync(ChoiceVFileSystemBlazorDatabaseContext dbContext, AccessLogsDbModel accessLog);
}