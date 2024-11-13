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
    
    [Get("/projects/{projectId}/tasks")]
    Task<ApiResponse<List<VikunjaTask>>> GetTasksInProjectAsync(int projectId, int page = 1, int limit = 50);
    
    [Get("/tasks/{tasksId}")]
    Task<ApiResponse<VikunjaTask>> GetTaskByIdAsync(int tasksId);
    
    [Get("/labels")]
    Task<ApiResponse<List<VikunjaLabel>>> GetAllLabelsAsync();
    
    [Put("/tasks/{taskId}/labels")]
    Task<ApiResponse<VikunjaAddLabelToTaskModel>> AddLabelToTaskAsync(int taskId, [Body] VikunjaAddLabelToTaskModel labelToTaskModel);

    [Get("/tasks/{taskId}/attachments")]
    Task<ApiResponse<List<VikunjaAttachment>>> GetAllAttachmentsForTaskAsync(int taskId);
    
    [Get("/tasks/{taskId}/comments")]
    Task<ApiResponse<List<VikunjaComment>>> GetAllCommentsForTaskAsync(int taskId);
    
    [Put("/tasks/{taskId}/comments")]
    Task<ApiResponse<VikunjaComment>> CreateNewTaskComment(int taskId, [Body] VikunjaComment newComment);
    
}