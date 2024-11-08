namespace ChoiceVFileSystemBlazor.Services.Vikunja.Models
{
    public record VikunjaUser(
        string Created,
        string Email,
        int Id,
        string Name,
        string Updated,
        string Username
    );
}