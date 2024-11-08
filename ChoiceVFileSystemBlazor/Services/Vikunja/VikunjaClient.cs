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
    
    [Put("/projects/{id}/tasks")]
    Task<ApiResponse<VikunjaTask>> CreateNewTask(int id, [Body] VikunjaTask task);
}