using System.Text.Json.Serialization;

namespace ChoiceVSharedApiModels.Whitelist;

public class WhitelistProceduresLogApiModel
{
    public WhitelistProceduresLogApiModel() { }

    public WhitelistProceduresLogApiModel(
        int id,
        ulong userId,
        ulong channelId,
        int step,
        string title,
        string message,
        DateTime date,
        int level)
    {
        Id = id;
        UserId = userId;
        ChannelId = channelId;
        Step = step;
        Title = title;
        Message = message;
        Date = date;
        Level = level;
    }
    
    [JsonPropertyName("id")]
    public int Id { get; set; }

    [JsonPropertyName("userId")]
    public ulong UserId { get; set; }

    [JsonPropertyName("channelId")]
    public ulong ChannelId { get; set; }

    [JsonPropertyName("step")]
    public int Step { get; set; }

    [JsonPropertyName("title")]
    public string Title { get; set; }

    [JsonPropertyName("message")]
    public string Message { get; set; }

    [JsonPropertyName("date")]
    public DateTime Date { get; set; }

    [JsonPropertyName("level")]
    public int Level { get; set; }
}
