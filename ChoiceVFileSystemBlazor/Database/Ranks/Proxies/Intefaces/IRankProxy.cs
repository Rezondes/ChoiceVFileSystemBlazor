using ChoiceVFileSystemBlazor.Database._Shared;
using ChoiceVFileSystemBlazor.Database.Ranks.DbModels;
using ChoiceVSharedApiModels.Accounts;

namespace ChoiceVFileSystemBlazor.Database.Ranks.Proxies.Intefaces;

public interface IRankProxy
{
    public Task<List<RightToRankDbModel>> GetAllRightsAsync(RankEnum rank);
    public Task<RightToRankDbModel?> AddRightToRankAsync(RightToRankDbModel rightToRankDbModel);
    public Task<bool> DeleteRightToRankAsync(RankEnum rank, RightEnum right);
    public Task<(int, int, int)> ChangeBulkAsync(Dictionary<(RankEnum, RightEnum),bool> values);
}