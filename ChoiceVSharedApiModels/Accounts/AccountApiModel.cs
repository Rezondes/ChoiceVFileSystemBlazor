using System.Text.Json.Serialization;

namespace ChoiceVSharedApiModels.Accounts;

public class AccountApiModel
{
    public AccountApiModel() {}
    
    public AccountApiModel(
        int id, 
        string name,
        string socialClubName,
        string discordId, 
        DateTime? lastLogin, 
        AccountStateEnum state, 
        string? stateReason)
    {
        Id = id;
        Name = name;
        SocialClubName = socialClubName;
        DiscordId = discordId;
        LastLogin = lastLogin;
        State = state;
        StateReason = stateReason;
    }

    [JsonPropertyName("id")]
    public int Id { get; set; }
    
    [JsonPropertyName("name")]
    public string Name { get; set; } = null!;

    [JsonPropertyName("socialClubName")] 
    public string SocialClubName { get; set; } = null!;

    [JsonPropertyName("discordId")] 
    public string DiscordId { get; set; } = null!;
    
    [JsonPropertyName("lastLogin")]
    public DateTime? LastLogin { get; set; }
    
    [JsonPropertyName("state")]
    public AccountStateEnum State { get; set; }
    
    [JsonPropertyName("stateReason")]
    public string? StateReason { get; set; }
}