using ChoiceVFileSystemBlazor.Database.BugTracker.DbModels;
using ChoiceVFileSystemBlazor.Database.BugTracker.Proxies.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace ChoiceVFileSystemBlazor.Database.BugTracker.Proxies;

public class BugTrackerProxy(IDbContextFactory<ChoiceVFileSystemBlazorDatabaseContext> dbContextFactory) : IBugTrackerProxy
{
    public async Task<List<BugTrackerTaskItemDbModel>> GetAllAsync()
    {
        await using var dbContext = await dbContextFactory.CreateDbContextAsync();
        return await dbContext.BugTrackerTaskItemDbModels.AsNoTracking().ToListAsync();
    }

    public async Task<BugTrackerTaskItemDbModel?> GetByIdAsync(Ulid id)
    {
        await using var dbContext = await dbContextFactory.CreateDbContextAsync();
        return await dbContext.BugTrackerTaskItemDbModels.AsNoTracking()
            .Include(t => t.Comments)
            .FirstOrDefaultAsync(x => x.Id == id);
    }

    public async Task<bool> AddAsync(BugTrackerTaskItemDbModel task)
    {
        await using var dbContext = await dbContextFactory.CreateDbContextAsync();
        await dbContext.BugTrackerTaskItemDbModels.AddAsync(task);
        return await dbContext.SaveChangesAsync() > 0;
    }

    public async Task<bool> SoftDeleteAsync(BugTrackerTaskItemDbModel task)
    {
        await using var dbContext = await dbContextFactory.CreateDbContextAsync();
        var existingTask = await dbContext.BugTrackerTaskItemDbModels.FirstOrDefaultAsync(x => x.Id == task.Id);
        if (existingTask == null) return false;

        existingTask.Deleted = true;
        return await dbContext.SaveChangesAsync() > 0;
    }

    public async Task<bool> RestoreAsync(BugTrackerTaskItemDbModel task)
    {
        await using var dbContext = await dbContextFactory.CreateDbContextAsync();
        var existingTask = await dbContext.BugTrackerTaskItemDbModels.FirstOrDefaultAsync(x => x.Id == task.Id);
        if (existingTask == null) return false;

        existingTask.Deleted = false;
        return await dbContext.SaveChangesAsync() > 0;
    }

    public async Task<bool> UpdateAsync(BugTrackerTaskItemDbModel task)
    {
        await using var dbContext = await dbContextFactory.CreateDbContextAsync();
        dbContext.BugTrackerTaskItemDbModels.Update(task);
        return await dbContext.SaveChangesAsync() > 0;
    }

    public async Task<bool> AddCommentAsync(Ulid taskId, BugTrackerTaskCommentDbModel comment)
    {
        await using var dbContext = await dbContextFactory.CreateDbContextAsync();
        var task = await dbContext.BugTrackerTaskItemDbModels.Include(t => t.Comments)
            .FirstOrDefaultAsync(t => t.Id == taskId);
        if (task == null) return false;

        task.Comments.Add(comment);
        return await dbContext.SaveChangesAsync() > 0;
    }

    public async Task<bool> SoftDeleteCommentAsync(Ulid commentId)
    {
        await using var dbContext = await dbContextFactory.CreateDbContextAsync();
        var comment = await dbContext.BugTrackerTaskItemDbModels.SelectMany(t => t.Comments)
            .FirstOrDefaultAsync(c => c.Id == commentId);
        if (comment == null) return false;

        comment.Deleted = true;
        return await dbContext.SaveChangesAsync() > 0;
    }

    public async Task<bool> RestoreCommentAsync(Ulid commentId)
    {
        await using var dbContext = await dbContextFactory.CreateDbContextAsync();
        var comment = await dbContext.BugTrackerTaskItemDbModels.SelectMany(t => t.Comments)
            .FirstOrDefaultAsync(c => c.Id == commentId);
        if (comment == null) return false;

        comment.Deleted = false;
        return await dbContext.SaveChangesAsync() > 0;
    }

    public async Task<bool> UpdateCommentAsync(BugTrackerTaskCommentDbModel comment)
    {
        await using var dbContext = await dbContextFactory.CreateDbContextAsync();
        dbContext.BugTrackerTaskCommentDbModels.Update(comment);
        return await dbContext.SaveChangesAsync() > 0;
    }
}