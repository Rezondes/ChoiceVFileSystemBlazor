using ChoiceVFileSystemBlazor.Components._Base;
using ChoiceVFileSystemBlazor.Components.Access.Pages;
using ChoiceVFileSystemBlazor.Components.Accounts.Pages;
using ChoiceVFileSystemBlazor.Components.Admin.Pages;
using ChoiceVFileSystemBlazor.Components.BankAccounts.Pages;
using ChoiceVFileSystemBlazor.Components.Characters.Pages;
using ChoiceVFileSystemBlazor.Components.Companies.Pages;
using ChoiceVFileSystemBlazor.Components.Groupingfiles.Pages;
using ChoiceVFileSystemBlazor.Components.Supportfiles.Pages;
using ChoiceVFileSystemBlazor.Components.Supportkeylogs.Pages;
using ChoiceVFileSystemBlazor.Database._Shared;

namespace ChoiceVFileSystemBlazor.Registrys;

public static class PageRightRegistry
{
    private static readonly Dictionary<string, RightEnum> PageRights = new()
    {
        { "", RightEnum.None },
        { Home.Url, RightEnum.None },
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
        { SupportfileCategoryControl.Url, RightEnum.ViewSupportfileCategoryControl },
        
        { BankAccountOverview.Url, RightEnum.ViewBankAccounts },
        
        { CharacterOverview.Url, RightEnum.ViewCharacters },
        { CharacterView.Url, RightEnum.ViewCharacters },
        { LiveCharacterOverview.Url, RightEnum.ViewCharacters },
        
        { CompanyOverview.Url, RightEnum.ViewCompanies },
        { CompanyView.Url, RightEnum.ViewCompanies },
        
        { GroupingfileCreate.Url, RightEnum.ViewGroupingfiles },
        { GroupingfileOverview.Url, RightEnum.ViewGroupingfiles },
        
        { SupportfileCreate.Url, RightEnum.SupportfileCreate },
        { SupportfileOverview.Url, RightEnum.ViewSupportfiles },
        { SupportfileView.Url, RightEnum.ViewSupportfiles },
        
        { SupportkeylogsOverview.Url, RightEnum.ViewSupportKeyLogsArea },
    };

    public static RightEnum GetNeededRankForPage(string relativePath)
    {
        return PageRights.GetValueOrDefault("/" + relativePath, RightEnum.None);
    }
}