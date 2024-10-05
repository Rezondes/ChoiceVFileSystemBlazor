using ChoiceVFileSystemBlazor.Database.Supportfiles.DbModels;
using ChoiceVFileSystemBlazor.Database.Supportfiles.Proxies.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace ChoiceVFileSystemBlazor.Database.Supportfiles.Proxies;

public class FileCategoryProxy(IDbContextFactory<ChoiceVFileSystemBlazorDatabaseContext> dbContextFactory) : IFileCategoryProxy
{
    public async Task<FileCategoryDbModel?> AddAsync(FileCategoryDbModel entity)
    {
        await using var dbContext = await dbContextFactory.CreateDbContextAsync();
        dbContext.Set<FileCategoryDbModel>().Add(entity);
        var changes = await dbContext.SaveChangesAsync();
        return changes <= 0 ? null : entity;
    }

    public async Task<FileCategoryDbModel?> GetAsync(Ulid id)
    {
        await using var dbContext = await dbContextFactory.CreateDbContextAsync();
        return await dbContext.Set<FileCategoryDbModel>().FindAsync(id);
    }

    public async Task<List<FileCategoryDbModel>> GetAllAsync()
    {
        await using var dbContext = await dbContextFactory.CreateDbContextAsync();
        return await dbContext.Set<FileCategoryDbModel>().ToListAsync();
    }

    public async Task<bool> UpdateAsync(FileCategoryDbModel entity)
    {
        await using var dbContext = await dbContextFactory.CreateDbContextAsync();
        dbContext.Set<FileCategoryDbModel>().Update(entity);
        var changes = await dbContext.SaveChangesAsync();
        return changes > 0;
    }

    public async Task<bool> DeleteAsync(Ulid id)
    {
        await using var dbContext = await dbContextFactory.CreateDbContextAsync();
        var entity = await dbContext.Set<FileCategoryDbModel>().FindAsync(id);
        if (entity == null) return false;
        
        dbContext.Set<FileCategoryDbModel>().Remove(entity);
        var changes = await dbContext.SaveChangesAsync();
        return changes > 0;
    }
}