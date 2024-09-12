using ChoiceVFileSystemBlazor.Database._Shared;

namespace ChoiceVFileSystemBlazor.Database.Ranks.DbModels;

public class RightToRankDbModel
{
    public Ulid Id { get; set; } = Ulid.NewUlid();
    public RankEnum Rank { get; set; }
    public RightEnum Right { get; set; }
}