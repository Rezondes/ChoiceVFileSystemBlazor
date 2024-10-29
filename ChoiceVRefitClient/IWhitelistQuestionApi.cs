using ChoiceVSharedApiModels.Whitelist;
using Refit;

namespace ChoiceVRefitClient;

public interface IWhitelistQuestionApi : IBaseApiInterface
{
    [Get("/api/v1/question")]
    Task<ApiResponse<List<WhitelistQuestionApiModel>>> GetAllAsync(CancellationToken cancellationToken = default); 
    
    [Get("/api/v1/question?questionId={questionId}")]
    Task<ApiResponse<WhitelistQuestionApiModel?>> GetByQuestionIdAsync(uint questionId, CancellationToken cancellationToken = default); 
}