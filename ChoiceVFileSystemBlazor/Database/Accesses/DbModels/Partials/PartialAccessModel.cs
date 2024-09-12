using ChoiceVFileSystemBlazor.Database._Shared;

namespace ChoiceVFileSystemBlazor.Database.Accesses.DbModels.Partials;

public class PartialAccessModel
{    
    public PartialAccessModel() {}
    
    public PartialAccessModel(int accountId, string discordId, string name)
    {
        AccountId = accountId;
        DiscordId = discordId;
        Name = name;
    }
    
    public PartialAccessModel(int accountId, string discordId, string name, RankEnum rank)
    {
        AccountId = accountId;
        DiscordId = discordId;
        Name = name;
        Rank = rank;
    }
    
    public int AccountId { get; set; }
    public string DiscordId { get; set; }
    public string Name { get; set; }
    public RankEnum Rank { get; set; } = RankEnum.None;
}