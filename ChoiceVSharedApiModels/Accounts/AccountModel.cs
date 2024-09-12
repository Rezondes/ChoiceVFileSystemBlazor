using System.Text.Json.Serialization;

namespace ChoiceVSharedApiModels.Accounts;

public class AccountModel
{
    [JsonPropertyName("id")]
    public int Id { get; set; }
    
    [JsonPropertyName("name")]
    public string Name { get; set; } = null!;

    [JsonPropertyName("socialClubName")] 
    public string SocialClubName { get; set; } = null!;

    [JsonPropertyName("discordId")] 
    public string DiscordId { get; set; } = null!;
    
    [JsonPropertyName("lastLogin")]
    public DateTime LastLogin { get; set; }
    
    [JsonPropertyName("state")]
    public int State { get; set; } // TODO Enum?
    
    [JsonPropertyName("stateReason")]
    public string? StateReason { get; set; }
}