using System.Text.Json.Serialization;

namespace ChoiceVSharedApiModels.Whitelist;

public class WhitelistQuestionApiModel
{
    public WhitelistQuestionApiModel() { }

    public WhitelistQuestionApiModel(
        uint id,
        string question,
        string answer1,
        string answer2,
        string answer3,
        string answer4,
        string answer5,
        bool answer1Right,
        bool answer2Right,
        bool answer3Right,
        bool answer4Right,
        bool answer5Right,
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
        Answer1Right = answer1Right;
        Answer2Right = answer2Right;
        Answer3Right = answer3Right;
        Answer4Right = answer4Right;
        Answer5Right = answer5Right;
        WronglyAnsweredCounter = wronglyAnsweredCounter;
        Explanation = explanation;
    }

    [JsonPropertyName("id")]
    public uint Id { get; set; }

    [JsonPropertyName("question")]
    public string Question { get; set; }

    [JsonPropertyName("answer1")]
    public string Answer1 { get; set; }

    [JsonPropertyName("answer2")]
    public string Answer2 { get; set; }

    [JsonPropertyName("answer3")]
    public string Answer3 { get; set; }

    [JsonPropertyName("answer4")]
    public string Answer4 { get; set; }

    [JsonPropertyName("answer5")]
    public string Answer5 { get; set; }

    [JsonPropertyName("answer1Right")]
    public bool Answer1Right { get; set; }

    [JsonPropertyName("answer2Right")]
    public bool Answer2Right { get; set; }

    [JsonPropertyName("answer3Right")]
    public bool Answer3Right { get; set; }

    [JsonPropertyName("answer4Right")]
    public bool Answer4Right { get; set; }

    [JsonPropertyName("answer5Right")]
    public bool Answer5Right { get; set; }

    [JsonPropertyName("wronglyAnsweredCounter")]
    public int WronglyAnsweredCounter { get; set; }

    [JsonPropertyName("explanation")]
    public string Explanation { get; set; }
}