namespace ChoiceVFileSystemBlazor.Services.Vikunja.Models
{
    public record VikunjaSubscription(
        string Created,
        int Entity,
        int EntityId,
        int Id
    );
}