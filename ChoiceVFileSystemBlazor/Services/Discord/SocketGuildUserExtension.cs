using Discord.WebSocket;

namespace ChoiceVFileSystemBlazor.Services.Discord;

public static class SocketGuildUserExtension
{
    public static bool HasCitizenRole(this SocketGuildUser user, DiscordBotService discordBotService)
    {
        var citizenRoleId = discordBotService.GetCitizenRoleId();
        var citizenRole = user.Roles.FirstOrDefault(x => x.Id == citizenRoleId);
        return citizenRole is not null;
    }
}