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
    
    public static async Task<List<VikunjaAttachment>> GetAllAttachmentsAsync(this IVikunjaClient vikunjaApi, int taskId)
    {
        var attachmentsResponse = await vikunjaApi.GetAttachmentsAsync(taskId);

        if (!attachmentsResponse.IsSuccessStatusCode || attachmentsResponse.Content == null)
            return [];

        var attachments = attachmentsResponse.Content;

        foreach (var attachment in attachments)
        {
            var dataResponse = await vikunjaApi.DownloadAttachmentAsync(taskId, attachment.Id.Value);
            if (!dataResponse.IsSuccessStatusCode) continue;
            
            var attachmentData = await dataResponse.Content.ReadAsByteArrayAsync();
            attachment.File.Mime = attachmentData;
        }

        return attachments;
    }
}