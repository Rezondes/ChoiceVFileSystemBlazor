using System.Text.Json.Serialization;

namespace ChoiceVSharedApiModels.Characters;

public class CharacterApiModel
{
    [JsonPropertyName("id")]
    public int Id { get; set; }
    
    [JsonPropertyName("account_id")]
    public int AccountId { get; set; }
    
    [JsonPropertyName("title")]
    public string? Title { get; set; }
    
    [JsonPropertyName("first_name")]
    public string FirstName { get; set; }
    
    [JsonPropertyName("last_name")]
    public string LastName { get; set; }
    
    [JsonPropertyName("middle_name")]
    public string MiddleName { get; set; }
    
    [JsonPropertyName("hunger")]
    public double Hunger { get; set; }
    
    [JsonPropertyName("thirst")]
    public double Thirst { get; set; }
    
    [JsonPropertyName("energy")]
    public double Energy { get; set; }
    
    [JsonPropertyName("health")]
    public double Health { get; set; }
        
    [JsonPropertyName("birth_date")]
    public DateTime BirthDate { get; set; }
        
    [JsonPropertyName("position")]
    public string Position { get; set; }
        
    [JsonPropertyName("rotation")]
    public string Rotation { get; set; }
        
    [JsonPropertyName("gender")]
    public char Gender { get; set; }
        
    [JsonPropertyName("cash")]
    public decimal Cash { get; set; }
        
    [JsonPropertyName("last_login")]
    public DateTime LastLogin { get; set; }
        
    [JsonPropertyName("last_logout")]
    public DateTime LastLogout { get; set; }
        
    [JsonPropertyName("dimension")]
    public int Dimension { get; set; }
}