namespace ChoiceVFileSystemBlazor.Helper;

public static class RouteHelper
{
    public static Dictionary<string, object> GetRouteValues(this Type pageType)
    {
        return new Dictionary<string, object>
        {
            { "page", pageType }
        };
    }
}