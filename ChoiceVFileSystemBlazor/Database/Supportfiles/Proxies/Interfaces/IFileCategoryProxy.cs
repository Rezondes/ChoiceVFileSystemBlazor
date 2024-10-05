using ChoiceVFileSystemBlazor.Database.Supportfiles.DbModels;

namespace ChoiceVFileSystemBlazor.Database.Supportfiles.Proxies.Interfaces;

public interface IFileCategoryProxy
{
    Task<FileCategoryDbModel?> AddAsync(FileCategoryDbModel entity);
    Task<FileCategoryDbModel?> GetAsync(Ulid id);
    Task<List<FileCategoryDbModel>> GetAllAsync();
    Task<bool> UpdateAsync(FileCategoryDbModel entity);
    Task<bool> DeleteAsync(Ulid id);
}