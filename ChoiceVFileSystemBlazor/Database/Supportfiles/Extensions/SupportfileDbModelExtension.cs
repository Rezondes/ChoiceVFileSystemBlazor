using ChoiceVFileSystemBlazor.Database.Supportfiles.DbModels;

namespace ChoiceVFileSystemBlazor.Database.Supportfiles.Extensions;

public static class SupportfileDbModelExtension
{
    public static SupportfileDbModel CreateShallowCopy(this SupportfileDbModel source)
    {
        return new SupportfileDbModel
        {
            Title = source.Title,
            Description = source.Description,
            Status = source.Status,
            MinRank = source.MinRank,
        };
    }
}