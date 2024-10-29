using ChoiceVSharedApiModels.Accounts;
using ChoiceVSharedApiModels.BankAccounts;
using Refit;

namespace ChoiceVRefitClient;

public interface IBankAccountApi : IBaseApiInterface
{
    [Get("/api/v1/bankaccount")]
    Task<ApiResponse<List<BankAccountApiModel>>> GetAllAsync(); 
    
    [Get("/api/v1/bankaccount?bankaccountId={bankaccountId}")]
    Task<ApiResponse<BankAccountApiModel?>> GetBankAccountByIdAsync(int bankaccountId); 
    
    [Get("/api/v1/bankaccount?characterId={characterId}")]
    Task<ApiResponse<List<BankAccountApiModel>>> GetBankAccountsByCharacterIdAsync(int characterId); 
    
    [Get("/api/v1/bankaccount?companyId={companyId}")]
    Task<ApiResponse<List<BankAccountApiModel>>> GetBankAccountsByCompanyIdAsync(int companyId); 
}