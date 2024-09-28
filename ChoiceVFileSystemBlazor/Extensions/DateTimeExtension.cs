using System.Globalization;

namespace ChoiceVFileSystemBlazor.Extensions;

public static class DateTimeExtension
{
    public static string ConvertTimeFromUtcWithTimeZone(this DateTime dateTime, string timeZoneId)
    {
        try
        {
            var timeZone = TimeZoneInfo.FindSystemTimeZoneById(timeZoneId);
        
            var localDateTime = TimeZoneInfo.ConvertTimeFromUtc(dateTime, timeZone);

            var culture = GetCultureForTimeZone(timeZoneId);
            
            var formattedDateTime = localDateTime.ToString(culture);

            return formattedDateTime;
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

    public static string ConvertTimeFromUtcWithTimeZone(this DateTime? dateTime, string timeZoneId)
    {
        if (!dateTime.HasValue) throw new ArgumentNullException(nameof(dateTime));

        return dateTime.Value.ConvertTimeFromUtcWithTimeZone(timeZoneId);
    }

    public static string? TryConvertTimeFromUtcWithTimeZone(this DateTime dateTime, string timeZoneId)
    {
        try
        {
            return ConvertTimeFromUtcWithTimeZone(dateTime, timeZoneId);
        }
        catch (TimeZoneNotFoundException)
        {
            return null;
        }
        catch (InvalidTimeZoneException)
        {
            return null;
        }
        catch (Exception)
        {
            return null;
        }
    }
    
    private static CultureInfo GetCultureForTimeZone(string timeZoneId)
    {
        // Eine Dictionary-Zuordnung von Zeitzonen zu den entsprechenden CultureInfo
        var timeZoneCultureMap = new Dictionary<string, string>()
        {
            // Europa
            { "Europe/Berlin", "de-DE" },        // Deutschland
            { "Europe/London", "en-GB" },        // Vereinigtes Königreich
            { "Europe/Paris", "fr-FR" },         // Frankreich
            { "Europe/Madrid", "es-ES" },        // Spanien
            { "Europe/Rome", "it-IT" },          // Italien
            { "Europe/Moscow", "ru-RU" },        // Russland

            // Nordamerika
            { "America/New_York", "en-US" },     // USA (Ostküste)
            { "America/Los_Angeles", "en-US" },  // USA (Westküste)
            { "America/Toronto", "en-CA" },      // Kanada (englisch)
            { "America/Vancouver", "en-CA" },    // Kanada (englisch)
            { "America/Mexico_City", "es-MX" },  // Mexiko

            // Südamerika
            { "America/Sao_Paulo", "pt-BR" },    // Brasilien (Portugiesisch)
            { "America/Argentina/Buenos_Aires", "es-AR" },  // Argentinien

            // Asien
            { "Asia/Tokyo", "ja-JP" },           // Japan
            { "Asia/Shanghai", "zh-CN" },        // China (vereinfacht)
            { "Asia/Seoul", "ko-KR" },           // Südkorea
            { "Asia/Kolkata", "hi-IN" },         // Indien (Hindi)

            // Australien und Ozeanien
            { "Australia/Sydney", "en-AU" },     // Australien
            { "Pacific/Auckland", "en-NZ" },     // Neuseeland

            // Afrika
            { "Africa/Johannesburg", "en-ZA" },  // Südafrika
            { "Africa/Cairo", "ar-EG" },         // Ägypten (Arabisch)

            // Mittlerer Osten
            { "Asia/Dubai", "ar-AE" },           // Vereinigte Arabische Emirate
        };

        return timeZoneCultureMap.TryGetValue(timeZoneId, value: out var value) ? 
            new CultureInfo(value) : 
            CultureInfo.InvariantCulture;
    }
}