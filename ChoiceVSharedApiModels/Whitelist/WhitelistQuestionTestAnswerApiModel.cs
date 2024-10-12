using System.Text.Json.Serialization;

namespace ChoiceVSharedApiModels.Whitelist;

public class WhitelistQuestionTestAnswerApiModel
{
    public WhitelistQuestionTestAnswerApiModel() { }


    public WhitelistQuestionTestAnswerApiModel(
        int whitelistTestId,
        uint questionId,
        string messageId,
        bool? answer_1,
        bool? answer_2,
        bool? answer_3,
        bool? answer_4,
        bool? answer_5,
        bool answered,
        WhitelistQuestionApiModel question)
    {
        WhitelistTestId = whitelistTestId;
        QuestionId = questionId;
        MessageId = messageId;
        SelectedAnswer1 = answer_1;
        SelectedAnswer2 = answer_2;
        SelectedAnswer3 = answer_3;
        SelectedAnswer4 = answer_4;
        SelectedAnswer5 = answer_5;
        Answered = answered;
        Question = question;
    }

    [JsonPropertyName("whitelistTestId")]
    public int WhitelistTestId { get; set; }

    [JsonPropertyName("questionId")]
    public uint QuestionId { get; set; }

    [JsonPropertyName("messageId")]
    public string MessageId { get; set; }

    [JsonPropertyName("answer1")]
    public bool? SelectedAnswer1 { get; set; }

    [JsonPropertyName("answer2")]
    public bool? SelectedAnswer2 { get; set; }

    [JsonPropertyName("answer3")]
    public bool? SelectedAnswer3 { get; set; }

    [JsonPropertyName("answer4")]
    public bool? SelectedAnswer4 { get; set; }

    [JsonPropertyName("answer5")]
    public bool? SelectedAnswer5 { get; set; }

    [JsonPropertyName("answered")]
    public bool Answered { get; set; }
    
    [JsonPropertyName("question")]
    public WhitelistQuestionApiModel Question { get; set; }
}
