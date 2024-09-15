using ChoiceVFileSystemBlazor.Database.News.DbModels;

namespace ChoiceVFileSystemBlazor.Database.News.Proxies.Interfaces;

public interface INewsProxy
{
    public Task<List<NewsDbModel>> GetAllAsync();

    public Task<NewsDbModel?> GetAsync(Ulid id);

    public Task<NewsDbModel?> AddAsync(NewsDbModel news);
    
    public Task<bool> UpdateTitleAsync(Ulid id, string newTitle, Ulid accessId);
    
    public Task<bool> UpdateContentAsync(Ulid id, List<string> newDescription, Ulid accessId);

    public Task<bool> DeleteAsync(Ulid id, Ulid accessId);
}