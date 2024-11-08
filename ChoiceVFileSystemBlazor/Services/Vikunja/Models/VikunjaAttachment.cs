namespace ChoiceVFileSystemBlazor.Services.Vikunja.Models
{
    public record VikunjaAttachment(
        string Created,
        VikunjaUser CreatedBy,
        VikunjaFile File,
        int Id,
        int TaskId
    );
}