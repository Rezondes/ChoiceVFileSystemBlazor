namespace ChoiceVFileSystemBlazor.Extensions;

public static class DateTimeExtension
{
    public static DateTime ConvertTimeFromUtcWithTimeZone(this DateTime dateTime, string timeZoneId)
    {
        try
        {
            var timeZone = TimeZoneInfo.FindSystemTimeZoneById(timeZoneId);
        
            return TimeZoneInfo.ConvertTimeFromUtc(dateTime, timeZone);
        }
        catch (TimeZoneNotFoundException)
        {
            throw new ArgumentException($"Die Zeitzone '{timeZoneId}' wurde nicht gefunden.");
        }
        catch (InvalidTimeZoneException)
        {
            throw new ArgumentException($"Die Zeitzone '{timeZoneId}' ist ungültig.");
        }
    }
    
    public static DateTime? TryConvertTimeFromUtcWithTimeZone(this DateTime dateTime, string timeZoneId)
    {
        try
        {
            var timeZone = TimeZoneInfo.FindSystemTimeZoneById(timeZoneId);
        
            return TimeZoneInfo.ConvertTimeFromUtc(dateTime, timeZone);
        }
        catch (TimeZoneNotFoundException)
        {
            // throw new ArgumentException($"Die Zeitzone '{timeZoneId}' wurde nicht gefunden.");
        }
        catch (InvalidTimeZoneException)
        {
            // throw new ArgumentException($"Die Zeitzone '{timeZoneId}' ist ungültig.");
        }
        
        return null;
    }
}