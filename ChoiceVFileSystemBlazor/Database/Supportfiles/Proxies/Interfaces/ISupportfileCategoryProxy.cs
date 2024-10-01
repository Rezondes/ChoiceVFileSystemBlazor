using ChoiceVFileSystemBlazor.Database.Supportfiles.DbModels;

namespace ChoiceVFileSystemBlazor.Database.Supportfiles.Proxies.Interfaces;

public interface ISupportfileCategoryProxy
{
    Task<SupportfileCategoryDbModel?> AddAsync(SupportfileCategoryDbModel entity);
    Task<SupportfileCategoryDbModel?> GetAsync(Ulid id);
    Task<List<SupportfileCategoryDbModel>> GetAllAsync();
    Task<bool> UpdateAsync(SupportfileCategoryDbModel entity);
    Task<bool> DeleteAsync(Ulid id);
}