using ChoiceVFileSystemBlazor.Components._Base;
using ChoiceVFileSystemBlazor.Components._Ucp.Messenger;
using ChoiceVFileSystemBlazor.Components.Access.Pages;
using ChoiceVFileSystemBlazor.Components.Accounts.Pages;
using ChoiceVFileSystemBlazor.Components.Admin.Pages;
using ChoiceVFileSystemBlazor.Components.BankAccounts.Pages;
using ChoiceVFileSystemBlazor.Components.Characters.Pages;
using ChoiceVFileSystemBlazor.Components.Chats.Pages;
using ChoiceVFileSystemBlazor.Components.Companies.Pages;
using ChoiceVFileSystemBlazor.Components.Groupingfiles.Pages;
using ChoiceVFileSystemBlazor.Components.Supportfiles.Pages;
using ChoiceVFileSystemBlazor.Components.Supportkeylogs.Pages;
using ChoiceVFileSystemBlazor.Components.WhitelistQuestions.Pages;
using ChoiceVFileSystemBlazor.Database._Shared;

namespace ChoiceVFileSystemBlazor.Registrys;

public static class PageRightRegistry
{
    private static readonly Dictionary<string, RightEnum> PageRights = new()
    {
        { "", RightEnum.None },
        { MainHome.Url, RightEnum.None },
        { NotAuthorized.Url, RightEnum.None },
        { Error.Url, RightEnum.None },
        { NotFound.Url, RightEnum.None },
        
        { AccessView.Url, RightEnum.None },
        
        { AccountOverview.Url, RightEnum.ViewAccounts },
        { AccountView.Url, RightEnum.ViewAccounts },
        
        { AccessControl.Url, RightEnum.ViewAdminAccessControl },
        { AccessLogs.Url, RightEnum.ViewAdminAccessLogs },
        { DiscordRolesControl.Url, RightEnum.ViewDiscordRolesControl },
        { NewsControl.Url, RightEnum.ViewNewsControl },
        { SupportfileCategoryControl.Url, RightEnum.ViewFileCategoryControl },
        
        { BankAccountOverview.Url, RightEnum.ViewBankAccounts },
        
        { CharacterOverview.Url, RightEnum.ViewCharacters },
        { CharacterView.Url, RightEnum.ViewCharacters },
        { LiveCharacterOverview.Url, RightEnum.ViewCharacters },
        
        { CompanyOverview.Url, RightEnum.ViewCompanies },
        { CompanyView.Url, RightEnum.ViewCompanies },
        
        { GroupingfileCreate.Url, RightEnum.GroupingfileCreate },
        { GroupingfileOverview.Url, RightEnum.ViewGroupingfiles },
        { GroupingfileView.Url, RightEnum.ViewGroupingfiles },
        
        { SupportfileCreate.Url, RightEnum.SupportfileCreate },
        { SupportfileOverview.Url, RightEnum.ViewSupportfiles },
        { SupportfileView.Url, RightEnum.ViewSupportfiles },
        
        { SupportkeylogsOverview.Url, RightEnum.ViewSupportKeyLogsArea },
        
        { WhitelistQuestionsOverview.Url, RightEnum.ViewWhitelistProcedures },
        { WhitelistQuestionsView.Url, RightEnum.ViewWhitelistQuestions },
        
        { MessengerChatsOverview.Url, RightEnum.None },
        { MessengerChat.Url, RightEnum.None },
    };

    public static RightEnum GetNeededRankForPage(string relativePath)
    {
        return PageRights.GetValueOrDefault("/" + relativePath, RightEnum.None);
    }
}