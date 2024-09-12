using ChoiceVFileSystemBlazor.Database._Shared;

namespace ChoiceVFileSystemBlazor.Database.Discord.DbModels.Partials;

public class PartialDiscordRoleDbModel
{
    public PartialDiscordRoleDbModel() { }

    public PartialDiscordRoleDbModel(string name, ulong discordRoleId, RankEnum automaticRank)
    {
        Name = name;
        DiscordRoleId = discordRoleId;
        AutomaticRank = automaticRank;
    }
    
    public string Name { get; set; } = null!;
    public ulong DiscordRoleId { get; set; }
    public RankEnum AutomaticRank { get; set; }
}