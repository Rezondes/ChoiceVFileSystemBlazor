﻿using System.Text.Json.Serialization;

namespace ChoiceVSharedApiModels.BankAccounts;

public class BankAtmWithdrawApiModel {
    
    [JsonPropertyName("id")]
    public int Id { get; set; }

    [JsonPropertyName("fromBankAccountId")]
    public long FromBankAccountId { get; set; }

    [JsonPropertyName("atmId")]
    public int AtmId { get; set; }

    [JsonPropertyName("amount")]
    public decimal Amount { get; set; }

    [JsonPropertyName("cost")]
    public decimal Cost { get; set; }

    [JsonPropertyName("date")]
    public DateTime Date { get; set; }
}
