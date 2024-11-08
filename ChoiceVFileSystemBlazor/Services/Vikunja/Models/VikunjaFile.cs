namespace ChoiceVFileSystemBlazor.Services.Vikunja.Models
{
    public record VikunjaFile(
        string Created,
        int Id,
        string Mime,
        string Name,
        int Size
    );
}