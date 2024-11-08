using ChoiceVFileSystemBlazor.Services.Vikunja.Models;

namespace ChoiceVFileSystemBlazor.Services.Vikunja.Models;

public record VikunjaTask
{
    public List<VikunjaUser> Assignees { get; init; }
    public List<VikunjaAttachment> Attachments { get; init; }
    public int BucketId { get; init; }
    public int CoverImageAttachmentId { get; init; }
    public string Created { get; init; }
    public VikunjaUser CreatedBy { get; init; }
    public string Description { get; init; }
    public bool Done { get; init; }
    public string DoneAt { get; init; }
    public string DueDate { get; init; }
    public string EndDate { get; init; }
    public string HexColor { get; init; }
    public int Id { get; init; }
    public string Identifier { get; init; }
    public int Index { get; init; }
    public bool IsFavorite { get; init; }
    public List<VikunjaLabel> Labels { get; init; }
    public int PercentDone { get; init; }
    public int Position { get; init; }
    public int Priority { get; init; }
    public int ProjectId { get; init; }
    public Dictionary<string, List<VikunjaUser>> Reactions { get; init; }
    public Dictionary<string, List<object>> RelatedTasks { get; init; }
    public List<VikunjaReminder> Reminders { get; init; }
    public int RepeatAfter { get; init; }
    public int RepeatMode { get; init; }
    public string StartDate { get; init; }
    public VikunjaSubscription Subscription { get; init; }
    public string Title { get; init; }
    public string Updated { get; init; }

    public VikunjaTask() { }

    public VikunjaTask(string title, string description)
    {
        Title = title;
        Description = description;
    }
}