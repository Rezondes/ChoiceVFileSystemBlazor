using System.Text.Json.Serialization;

namespace ChoiceVSharedApiModels.Whitelist;

public class WhitelistProcedureDataApiModel
{
    public WhitelistProcedureDataApiModel() { }

    public WhitelistProcedureDataApiModel(
        ulong userId,
        ulong channelId,
        string name,
        ulong messageId,
        string data,
        bool? isInEdit,
        bool finished)
    {
        UserId = userId;
        ChannelId = channelId;
        Name = name;
        MessageId = messageId;
        Data = data;
        IsInEdit = isInEdit;
        Finished = finished;
    }
    
    [JsonPropertyName("userId")]
    public ulong UserId { get; set; }

    [JsonPropertyName("channelId")]
    public ulong ChannelId { get; set; }

    [JsonPropertyName("name")]
    public string Name { get; set; }

    [JsonPropertyName("messageId")]
    public ulong MessageId { get; set; }

    [JsonPropertyName("data")]
    public string Data { get; set; }

    [JsonPropertyName("isInEdit")]
    public bool? IsInEdit { get; set; }

    [JsonPropertyName("finished")]
    public bool Finished { get; set; }
}
