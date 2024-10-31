using ChoiceVFileSystemBlazor.Database.BugTracker.DbModels;

namespace ChoiceVFileSystemBlazor.Database.BugTracker.Proxies.Interfaces;

public interface IBugTrackerProxy
{
    public Task<List<BugTrackerTaskItemDbModel>> GetAllAsync();

    public Task<BugTrackerTaskItemDbModel?> GetByIdAsync(Ulid id);
    
    public Task<bool> AddAsync(BugTrackerTaskItemDbModel task);
    
    public Task<bool> SoftDeleteAsync(BugTrackerTaskItemDbModel task);
    
    public Task<bool> RestoreAsync(BugTrackerTaskItemDbModel task);
    
    public Task<bool> UpdateAsync(BugTrackerTaskItemDbModel task);
    
    public Task<bool> AddCommentAsync(Ulid taskId, BugTrackerTaskCommentDbModel comment);
    
    public Task<bool> SoftDeleteCommentAsync(Ulid commentId);
    
    public Task<bool> RestoreCommentAsync(Ulid commentId);
    
    public Task<bool> UpdateCommentAsync(BugTrackerTaskCommentDbModel comment);
}