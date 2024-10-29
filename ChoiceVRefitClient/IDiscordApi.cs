using ChoiceVSharedApiModels.Accounts;
using ChoiceVSharedApiModels.Discord;
using Refit;

namespace ChoiceVRefitClient;

public interface IDiscordApi : IBaseApiInterface
{
    [Get("/api/v1/discord")]
    Task<ApiResponse<List<DiscordUserApiModel>>> GetAllDiscordGuildMembersAsync(CancellationToken cancellationToken = default); 
}