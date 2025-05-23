﻿using ChoiceVFileSystemBlazor.Database._Shared;
using ChoiceVFileSystemBlazor.Database.Accesses.DbModels.Partials;
using ChoiceVFileSystemBlazor.Database.News.DbModels;
using ChoiceVFileSystemBlazor.Database.Supportfiles.DbModels;

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
    public AccessSettingsDbModel Settings { get; set; }
    public List<FileDbModel> Supportfiles { get; set; } = [];
    public List<FileLogsDbModel> SupportfileLogs { get; set; } = [];
    public List<FileEntryDbModel> SupportfileEntrys { get; set; } = [];
    public List<AccessLogsDbModel> CreatedAccessLogs { get; set; } = [];
    public List<AccessLogsDbModel> TargetedAccessLogs { get; set; } = [];
    public List<NewsDbModel> NewsDbModels { get; set; } = [];
}