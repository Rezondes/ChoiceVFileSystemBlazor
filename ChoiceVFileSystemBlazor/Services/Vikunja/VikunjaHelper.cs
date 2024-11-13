using ChoiceVFileSystemBlazor.Services.Vikunja.Models;

namespace ChoiceVFileSystemBlazor.Services.Vikunja;

public static class VikunjaHelper
{
    public static async Task<List<VikunjaTask>> GetAllTasksInProjectAsync(this IVikunjaClient vikunjaApi, int projectId)
    {
        var allTasks = new List<VikunjaTask>();
        var page = 1;
        const int limit = 50;

        while (true)
        {
            var response = await vikunjaApi.HandleApiRequestAsync(
                async _ => await vikunjaApi.GetTasksInProjectAsync(projectId, page, limit));

            if (response.IsSuccess)
            {
                allTasks.AddRange(response.Data);

                if (response.Data.Count < limit)
                {
                    break;
                }
            }
            else
            {
                break;
            }

            page++;
        }

        return allTasks;
    }

}