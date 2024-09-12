using System.Text.Json.Serialization;

namespace ChoiceVSharedApiModels.Inventory;

public class InventoryModel
{
    [JsonPropertyName("characterId")]
    public int CharacterId { get; set; }
    
    [JsonPropertyName("items")]
    public List<InventoryItemModel> Items { get; set; }
}