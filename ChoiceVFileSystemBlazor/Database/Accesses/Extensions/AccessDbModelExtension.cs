using ChoiceVFileSystemBlazor.Database.Accesses.DbModels;
using ChoiceVFileSystemBlazor.Database.Accesses.DbModels.Partials;

namespace ChoiceVFileSystemBlazor.Database.Accesses.Extensions;

public static class AccessDbModelExtension
{
    public static PartialAccessModel CreateShallowCopy(this PartialAccessModel source)
    {
        return new PartialAccessModel
        {
            Name = source.Name,
            AccountId = source.AccountId,
            DiscordId = source.DiscordId,
            Rank = source.Rank,
        };
    }
}