namespace CalendarApp.Domain.Entities;

using CalendarApp.Domain.Enums;

public class CalendarShare
{
    public int Id { get; set; }
    public ShareAccessLevel AccessLevel { get; set; } = ShareAccessLevel.ReadOnly;
    public DateTime SharedAt { get; set; } = DateTime.UtcNow;
    public bool IsAccepted { get; set; }

    // Foreign keys
    public int CalendarId { get; set; }
    public Calendar Calendar { get; set; } = null!;
    public string SharedWithUserId { get; set; } = string.Empty;
    public ApplicationUser SharedWithUser { get; set; } = null!;
}
