using ChoiceVFileSystemBlazor.Database.Supportfiles.DbModels;

namespace ChoiceVFileSystemBlazor.Database.Supportfiles.Extensions;

public static class SupportfileDbModelExtension
{
    public static FileDbModel CreateShallowCopy(this FileDbModel source)
    {
        return new FileDbModel
        {
            Title = source.Title,
            Description = source.Description,
            Status = source.Status,
            MinRank = source.MinRank,
            Type = source.Type,
        };
    }
}