using ChoiceVSharedApiModels.Whitelist;
using Refit;

namespace ChoiceVRefitClient;

public interface IWhitelistQuestionApi
{
    [Get("/api/v1/question")]
    Task<ApiResponse<List<WhitelistQuestionApiModel>>> GetAllAsync(); 
    
    [Get("/api/v1/question?questionId={questionId}")]
    Task<ApiResponse<WhitelistQuestionApiModel?>> GetByQuestionIdAsync(uint questionId); 
}