using ChoiceVFileSystemBlazor.Database._Shared;
using ChoiceVFileSystemBlazor.Database.Discord.DbModels.Partials;

namespace ChoiceVFileSystemBlazor.Database.Discord.DbModels;

public class DiscordRoleDbModel : PartialDiscordRoleDbModel
{
    public DiscordRoleDbModel() { }

    public DiscordRoleDbModel(string name, ulong discordRoleId, RankEnum automaticRank)
    {
        Name = name;
        DiscordRoleId = discordRoleId;
        AutomaticRank = automaticRank;
    }
    
    public Ulid Id { get; set; } = Ulid.NewUlid();
}