using System.Text.Json.Serialization;

namespace ChoiceVSharedApiModels.BankAccounts;

public class BankBankWithdrawApiModel {
    
    [JsonPropertyName("id")]
    public int Id { get; set; }

    [JsonPropertyName("fromBankAccountId")]
    public long FromBankAccountId { get; set; }

    [JsonPropertyName("amount")]
    public decimal Amount { get; set; }

    [JsonPropertyName("reason")]
    public string Reason { get; set; }

    [JsonPropertyName("date")]
    public DateTime Date { get; set; }
}
