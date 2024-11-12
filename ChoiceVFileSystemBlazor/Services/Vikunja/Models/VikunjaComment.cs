namespace ChoiceVFileSystemBlazor.Services.Vikunja.Models;

public record VikunjaComment(
    VikunjaAuthor Author,
    string Comment,
    string Created,
    int? Id,
    Dictionary<string, List<VikunjaReaction>> Reactions,
    string Updated
);