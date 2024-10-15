namespace ChoiceVFileSystemBlazor.Database._Shared;

public enum RankEnum
{
    None = 0,
    
    CommunityManagement = 51,
    Concepting = 52,
    Development = 53,
    
    SupportTrainee = 101,
    Supporter = 102,
    SeniorSupporter = 103,
    HeadOfSupport = 104,
    Admin = 1000
}

public static class RankEnumExtensions
{
    public static string GetDisplayText(this RankEnum rank)
    {
        return rank switch
        {
            RankEnum.None => "Kein Zugriff",
            RankEnum.CommunityManagement => "Community Management",
            RankEnum.Concepting => "Concepting",
            RankEnum.Development => "expr",
            RankEnum.SupportTrainee => "Support-Trainee",
            RankEnum.Supporter => "Supporter",
            RankEnum.SeniorSupporter => "Senior Supporter",
            RankEnum.HeadOfSupport => "Head of Support",
            RankEnum.Admin => "Administrator",
            _ => throw new ArgumentOutOfRangeException(nameof(rank), rank, null)
        };
    }
}