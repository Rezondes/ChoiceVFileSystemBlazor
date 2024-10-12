using System.Text.Json.Serialization;

namespace ChoiceVSharedApiModels.Whitelist;

public class WhitelistQuestionApiModel
{
    public WhitelistQuestionApiModel() { }

    public WhitelistQuestionApiModel(
        uint id,
        string question,
        WhitelistQuestionAnswerApiModel answer1,
        WhitelistQuestionAnswerApiModel answer2,
        WhitelistQuestionAnswerApiModel answer3,
        WhitelistQuestionAnswerApiModel answer4,
        WhitelistQuestionAnswerApiModel answer5,
        int wronglyAnsweredCounter,
        string explanation)
    {
        Id = id;
        Question = question;
        Answer1 = answer1;
        Answer2 = answer2;
        Answer3 = answer3;
        Answer4 = answer4;
        Answer5 = answer5;
        WronglyAnsweredCounter = wronglyAnsweredCounter;
        Explanation = explanation;
    }

    [JsonPropertyName("id")]
    public uint Id { get; set; }

    [JsonPropertyName("question")]
    public string Question { get; set; }

    [JsonPropertyName("answer1")]
    public WhitelistQuestionAnswerApiModel? Answer1 { get; set; }

    [JsonPropertyName("answer2")]
    public WhitelistQuestionAnswerApiModel? Answer2 { get; set; }

    [JsonPropertyName("answer3")]
    public WhitelistQuestionAnswerApiModel? Answer3 { get; set; }

    [JsonPropertyName("answer4")]
    public WhitelistQuestionAnswerApiModel? Answer4 { get; set; }

    [JsonPropertyName("answer5")]
    public WhitelistQuestionAnswerApiModel? Answer5 { get; set; }

    [JsonPropertyName("wronglyAnsweredCounter")]
    public int WronglyAnsweredCounter { get; set; }

    [JsonPropertyName("explanation")]
    public string Explanation { get; set; }
}

public record WhitelistQuestionAnswerApiModel(string Text, bool IsCorrect);