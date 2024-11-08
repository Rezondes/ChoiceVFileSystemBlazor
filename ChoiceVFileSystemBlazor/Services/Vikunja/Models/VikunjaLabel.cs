namespace ChoiceVFileSystemBlazor.Services.Vikunja.Models
{
    public record VikunjaLabel(
        string Created,
        VikunjaUser CreatedBy,
        string Description,
        string HexColor,
        int Id,
        string Title,
        string Updated
    );
}