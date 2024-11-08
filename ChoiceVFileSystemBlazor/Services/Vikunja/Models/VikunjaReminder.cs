namespace ChoiceVFileSystemBlazor.Services.Vikunja.Models
{
    public record VikunjaReminder(
        int RelativePeriod,
        string RelativeTo,
        string ReminderText
    );
}
