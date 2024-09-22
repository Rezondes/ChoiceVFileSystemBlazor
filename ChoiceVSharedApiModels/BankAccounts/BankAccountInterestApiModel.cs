using System.Text.Json.Serialization;

namespace ChoiceVSharedApiModels.BankAccounts;

public class BankAccountInterestApiModel {
    
    [JsonPropertyName("bankAccountId")]
    public long BankAccountId { get; set; }

    [JsonPropertyName("interestPercent")]
    public float InterestPercent { get; set; }

    [JsonPropertyName("interestAmount")]
    public decimal InterestAmount { get; set; }

    [JsonPropertyName("nextInterest")]
    public DateTime NextInterest { get; set; }
}
