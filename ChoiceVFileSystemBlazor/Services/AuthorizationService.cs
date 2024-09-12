using ChoiceVFileSystemBlazor.Database._Shared;
using ChoiceVFileSystemBlazor.Database.Accesses.DbModels;

namespace ChoiceVFileSystemBlazor.Services;

public class AuthorizationService
{
    public static bool IsAuthenticated(AccessDbModel? accessModel)
    {
        return accessModel is not null && accessModel.Rank > RankEnum.None;
    }

    public static bool HasRight(List<RightEnum> userRights, RankEnum userRank, RightEnum requiredRight)
    {
        return userRank == RankEnum.Admin || userRights.Contains(requiredRight);
    }
}