using System.Text.Json.Serialization;

namespace ChoiceVSharedApiModels.Whitelist;

public class WhitelistProcedureApiModel
{
    public WhitelistProcedureApiModel() { }

    public WhitelistProcedureApiModel(
        int id,
        ulong userId,
        ulong channelId,
        int currentStep,
        bool blocked,
        bool notCanceable,
        DateTime startTime,
        DateTime cancelStartTime,
        List<WhitelistProcedureDataApiModel>? dataList = null,
        List<WhitelistProceduresLogApiModel>? logList = null,
        List<WhitelistQuestionsTestApiModel>? testList = null
    )
    {
        Id = id;
        UserId = userId;
        ChannelId = channelId;
        CurrentStep = currentStep;
        Blocked = blocked;
        NotCanceable = notCanceable;
        StartTime = startTime;
        CancelStartTime = cancelStartTime;

        if (dataList is not null)
        {
            DataList = dataList;
        }
        if (logList is not null)
        {
            LogList = logList;
        }
        if (testList is not null)
        {
            TestList = testList;
        }
    }

    [JsonPropertyName("id")]
    public int Id { get; set; }

    [JsonPropertyName("userId")]
    public ulong UserId { get; set; }

    [JsonPropertyName("channelId")]
    public ulong ChannelId { get; set; }

    [JsonPropertyName("currentStep")]
    public int CurrentStep { get; set; }

    [JsonPropertyName("blocked")]
    public bool Blocked { get; set; }

    [JsonPropertyName("notCanceable")]
    public bool NotCanceable { get; set; }

    [JsonPropertyName("startTime")]
    public DateTime StartTime { get; set; }

    [JsonPropertyName("cancelStartTime")]
    public DateTime CancelStartTime { get; set; }
    
    [JsonPropertyName("dataList")]
    public List<WhitelistProcedureDataApiModel> DataList { get; set; } = [];
    
    [JsonPropertyName("logList")]
    public List<WhitelistProceduresLogApiModel> LogList { get; set; } = [];
    
    [JsonPropertyName("testList")]
    public List<WhitelistQuestionsTestApiModel> TestList { get; set; } = [];
}
