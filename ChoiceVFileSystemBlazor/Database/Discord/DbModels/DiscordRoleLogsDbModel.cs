using System.ComponentModel.DataAnnotations.Schema;
using ChoiceVFileSystemBlazor.Database.Discord.Enums;
using ChoiceVFileSystemBlazor.Extensions;

namespace ChoiceVFileSystemBlazor.Database.Discord.DbModels;

public class DiscordRoleLogsDbModel
{
    public DiscordRoleLogsDbModel() {}

    public DiscordRoleLogsDbModel(DiscordRoleLogTypeEnum type, Ulid accessId, string content)
    {
        Type = type;
        AccessId = accessId;
        Content = content;
    }
    
    public Ulid Id { get; set; } = Ulid.NewUlid();
    public DiscordRoleLogTypeEnum Type { get; set; }
    public Ulid AccessId { get; set; }
    public string Content { get; set; }
    
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public string CreatedAtLocal(string timeZoneId = "Europe/Berlin") => CreatedAt.ConvertTimeFromUtcWithTimeZone(timeZoneId);
}