using System.ComponentModel.DataAnnotations.Schema;
using ChoiceVFileSystemBlazor.Database._Shared;
using ChoiceVFileSystemBlazor.Database.Accesses.DbModels.Partials;
using ChoiceVFileSystemBlazor.Database.News.DbModels;
using ChoiceVFileSystemBlazor.Database.Supportfiles.DbModels;
using ChoiceVSharedApiModels.Accounts;

namespace ChoiceVFileSystemBlazor.Database.Accesses.DbModels;

public class AccessDbModel : PartialAccessModel
{
    public AccessDbModel(int accountId, string discordId, string name)
    {
        AccountId = accountId;
        DiscordId = discordId;
        Name = name;
    }
    
    public AccessDbModel(int accountId, string discordId, string name, RankEnum rank)
    {
        AccountId = accountId;
        DiscordId = discordId;
        Name = name;
        Rank = rank;
    }
    
    public Ulid Id { get; set; } = Ulid.NewUlid();
    
    // Navigation Properties
    public List<SupportfileDbModel> Supportfiles { get; set; } = [];
    public List<SupportfileLogsDbModel> SupportfileLogs { get; set; } = [];
    public List<SupportfileEntryDbModel> SupportfileEntrys { get; set; } = [];
    public List<AccessLogsDbModel> CreatedAccessLogs { get; set; } = [];
    public List<AccessLogsDbModel> TargetedAccessLogs { get; set; } = [];
    public List<NewsDbModel> NewsDbModels { get; set; } = [];
}