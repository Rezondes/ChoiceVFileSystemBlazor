using System.Reflection.Metadata.Ecma335;
using ChoiceVFileSystemBlazor.Database._Shared;

namespace ChoiceVFileSystemBlazor.Models;

public class InputModel(InputTypes type, string label, string value, string placeholder)
{
    public InputModel(InputTypes type, string label, string value, string placeholder, int lines) : this(type, label, value, placeholder)
    {
        Lines = lines;
    }
    
    // Select
    public InputModel(InputTypes type, string label, string value, string placeholder, IEnumerable<InputOptionModel> options, bool multiSelect = false) : this(type, label, value, placeholder)
    {
        Options = options;
        MultiSelect = multiSelect;
    }

    // InputTypes.Number
    public InputModel(InputTypes type, string label, string value, string placeholder, string min, string max) : this(type, label, value, placeholder)
    {
        Min = min;
        Max = max;
    }

    public InputTypes Type { get; set; } = type;
    public string Label { get; set; } = label;
    public string Value { get; set; } = value;
    public string Placeholder { get; set; } = placeholder;
    public string Min { get; set; }
    public string Max { get; set; }
    public int Lines { get; set; } = 1;
    public bool MultiSelect { get; set; } = false;
    public IEnumerable<InputOptionModel> Options { get; set; }
}

public enum InputTypes
{
    Text,
    Number,
    Hidden,
    Select
}

public class InputOptionModel(string value, string text)
{
    public string Value { get; set; } = value;
    public string Text { get; set; } = text;

    public static IEnumerable<InputOptionModel> GetOptionsForRankEnum()
    {
        var rankSelectOptions = new List<InputOptionModel>();
        foreach (RankEnum rank in Enum.GetValues(typeof(RankEnum)))
        {
            rankSelectOptions.Add(new(rank.ToString(), rank.ToString()));
        }

        return rankSelectOptions;
    }
}
