using ChoiceVFileSystemBlazor.Database.News.DbModels;
using ChoiceVFileSystemBlazor.Database.News.Proxies.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace ChoiceVFileSystemBlazor.Database.News.Proxies;

public class NewsProxy(IDbContextFactory<ChoiceVFileSystemBlazorDatabaseContext> dbContextFactory) : INewsProxy
{
    public async Task<List<NewsDbModel>> GetAllAsync()
    {
        await using var dbContext = await dbContextFactory.CreateDbContextAsync();
        
        return await dbContext.NewsDbModels
            .AsNoTracking()
            .Include(x => x.Creator)
            .ToListAsync();
    }

    public async Task<NewsDbModel?> GetAsync(Ulid id)
    {
        await using var dbContext = await dbContextFactory.CreateDbContextAsync();
        
        return await dbContext.NewsDbModels
            .AsNoTracking()
            .Include(x => x.Creator)
            .FirstOrDefaultAsync(x => x.Id == id);
    }

    public async Task<NewsDbModel?> AddAsync(NewsDbModel news)
    {
        await using var dbContext = await dbContextFactory.CreateDbContextAsync();
        
        await dbContext.NewsDbModels.AddAsync(news);
        // TODO add Logs
        var changes = await dbContext.SaveChangesAsync();
        
        return changes <= 0 ? null : news;
    }
    
    public async Task<bool> UpdateTitleAsync(Ulid id, string newTitle, Ulid accessId)
    {
        var news = await GetAsync(id);
        if (news is null) return false;
        
        await using var dbContext = await dbContextFactory.CreateDbContextAsync();

        var oldTitle = news.Title;
        
        news.Title = newTitle;
        
        dbContext.NewsDbModels.Update(news);
        // TODO add Logs
        var changes = await dbContext.SaveChangesAsync();
        
        return changes > 0;
    }
    
    public async Task<bool> UpdateContentAsync(Ulid id, List<string> newDescription, Ulid accessId)
    {
        var news = await GetAsync(id);
        if (news is null) return false;
        
        await using var dbContext = await dbContextFactory.CreateDbContextAsync();
        
        var oldDescription = news.Content;
        
        news.Content = newDescription; 
        
        dbContext.NewsDbModels.Update(news);
        // TODO add Logs
        var changes = await dbContext.SaveChangesAsync();
        
        return changes > 0;
    }

    public async Task<bool> DeleteAsync(Ulid id, Ulid accessId)
    {
        var news = await GetAsync(id);
        if (news is null) return false;
        
        await using var dbContext = await dbContextFactory.CreateDbContextAsync();
        
        dbContext.NewsDbModels.Remove(news);
        // TODO add Logs
        var changes = await dbContext.SaveChangesAsync();
        
        return changes > 0;
    }
}