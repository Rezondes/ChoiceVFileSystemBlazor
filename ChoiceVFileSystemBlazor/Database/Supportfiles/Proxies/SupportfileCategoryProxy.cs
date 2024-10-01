using ChoiceVFileSystemBlazor.Database.Supportfiles.DbModels;
using ChoiceVFileSystemBlazor.Database.Supportfiles.Proxies.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace ChoiceVFileSystemBlazor.Database.Supportfiles.Proxies;

public class SupportfileCategoryProxy(IDbContextFactory<ChoiceVFileSystemBlazorDatabaseContext> dbContextFactory) : ISupportfileCategoryProxy
{
    public async Task<SupportfileCategoryDbModel?> AddAsync(SupportfileCategoryDbModel entity)
    {
        await using var dbContext = await dbContextFactory.CreateDbContextAsync();
        dbContext.Set<SupportfileCategoryDbModel>().Add(entity);
        var changes = await dbContext.SaveChangesAsync();
        return changes <= 0 ? null : entity;
    }

    public async Task<SupportfileCategoryDbModel?> GetAsync(Ulid id)
    {
        await using var dbContext = await dbContextFactory.CreateDbContextAsync();
        return await dbContext.Set<SupportfileCategoryDbModel>().FindAsync(id);
    }

    public async Task<List<SupportfileCategoryDbModel>> GetAllAsync()
    {
        await using var dbContext = await dbContextFactory.CreateDbContextAsync();
        return await dbContext.Set<SupportfileCategoryDbModel>().ToListAsync();
    }

    public async Task<bool> UpdateAsync(SupportfileCategoryDbModel entity)
    {
        await using var dbContext = await dbContextFactory.CreateDbContextAsync();
        dbContext.Set<SupportfileCategoryDbModel>().Update(entity);
        var changes = await dbContext.SaveChangesAsync();
        return changes > 0;
    }

    public async Task<bool> DeleteAsync(Ulid id)
    {
        await using var dbContext = await dbContextFactory.CreateDbContextAsync();
        var entity = await dbContext.Set<SupportfileCategoryDbModel>().FindAsync(id);
        if (entity == null) return false;
        
        dbContext.Set<SupportfileCategoryDbModel>().Remove(entity);
        var changes = await dbContext.SaveChangesAsync();
        return changes > 0;
    }
}