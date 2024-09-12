using ChoiceVFileSystemBlazor.Database.Discord.Enums;

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
}