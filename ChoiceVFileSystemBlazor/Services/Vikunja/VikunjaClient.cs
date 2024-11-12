using ChoiceVFileSystemBlazor.Services.Vikunja.Models;
using ChoiceVRefitClient;
using Refit;

namespace ChoiceVFileSystemBlazor.Services.Vikunja;

public interface IVikunjaClient : IBaseApiInterface
{
    [Get("/tasks/all")]
    Task<ApiResponse<List<VikunjaTask>>> GetAllAsync();
    
    [Get("/routes")]
    Task<ApiResponse<List<VikunjaModel>>> GetRoutesAsync();
    
    [Put("/projects/{projectId}/tasks")]
    Task<ApiResponse<VikunjaTask>> CreateNewTaskAsync(int projectId, [Body] VikunjaTask task);

    [Get("/projects/{projectId}/views/{viewId}/tasks")]
    Task<ApiResponse<VikunjaTask>> GetTasksAsync(int projectId, int viewId);
    
    [Get("/tasks/{tasksId}")]
    Task<ApiResponse<VikunjaTask>> GetTaskByIdAsync(int tasksId);
}