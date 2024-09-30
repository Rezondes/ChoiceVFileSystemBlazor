namespace ChoiceVFileSystemBlazor.Database._Shared;

public enum RankEnum
{
    None,
    Rank1,
    Rank2,
    Rank3,
    Rank4,
    Admin
}

public static class RankEnumExtensions
{
    public static string GetDisplayText(this RankEnum rank)
    {
        return rank switch
        {
            RankEnum.None => "Kein Zugriff",
            RankEnum.Rank1 => "Support-Trainee",
            RankEnum.Rank2 => "Supporter",
            RankEnum.Rank3 => "Senior Supporter",
            RankEnum.Rank4 => "Head of Support",
            RankEnum.Admin => "Administrator",
            _ => throw new ArgumentOutOfRangeException(nameof(rank), rank, null)
        };
    }
}