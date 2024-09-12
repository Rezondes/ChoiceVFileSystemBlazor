using System.Text.Json.Serialization;

namespace ChoiceVSharedApiModels.Inventory;

public class InventoryItemModel
{
    [JsonPropertyName("id")]
    public int Id { get; set; }
    
    [JsonPropertyName("name")]
    public string Name { get; set; }
    
    [JsonPropertyName("description")]
    public string Description { get; set; }
    
    [JsonPropertyName("quantity")]
    public int Quantity { get; set; }
}