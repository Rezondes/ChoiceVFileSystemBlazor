using System.Text.Json.Serialization;

namespace ChoiceVSharedApiModels.Whitelist;

public class WhitelistQuestionsTestApiModel
{
    public WhitelistQuestionsTestApiModel() { }

    public WhitelistQuestionsTestApiModel(
        int id,
        ulong userId,
        ulong channelId,
        int wrongQuestions,
        int rightQuestions,
        bool finished,
        List<WhitelistQuestionTestAnswerApiModel> questionAnswers)
    {
        Id = id;
        UserId = userId;
        ChannelId = channelId;
        WrongQuestions = wrongQuestions;
        RightQuestions = rightQuestions;
        Finished = finished;
        QuestionAnswers = questionAnswers;
    }

    [JsonPropertyName("id")]
    public int Id { get; set; }

    [JsonPropertyName("userId")]
    public ulong UserId { get; set; }

    [JsonPropertyName("channelId")]
    public ulong ChannelId { get; set; }

    [JsonPropertyName("wrongQuestions")]
    public int WrongQuestions { get; set; }

    [JsonPropertyName("rightQuestions")]
    public int RightQuestions { get; set; }

    [JsonPropertyName("finished")]
    public bool Finished { get; set; }
    
    [JsonPropertyName("questionAnswers")]
    public List<WhitelistQuestionTestAnswerApiModel> QuestionAnswers { get; set; } = [];
}
