using ChoiceVSharedApiModels.Accounts;
using ChoiceVSharedApiModels.BankAccounts;
using Refit;

namespace ChoiceVRefitClient;

public interface IBankAccountApi
{
    [Get("/api/v1/bankaccount")]
    Task<ApiResponse<List<BankAccountApiModel>>> GetAllAsync(); 
}