namespace CalendarApp.Domain.Entities;

using CalendarApp.Domain.Enums;

public class Event
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string? Description { get; set; }
    public string? Location { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public bool IsAllDay { get; set; }
    public RecurrenceType Recurrence { get; set; } = RecurrenceType.None;
    public DateTime? RecurrenceEndDate { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

    // Foreign keys
    public int CalendarId { get; set; }
    public Calendar Calendar { get; set; } = null!;
    public int? CategoryId { get; set; }
    public Category? Category { get; set; }

    // Navigation
    public ICollection<Notification> Notifications { get; set; } = new List<Notification>();
    public ICollection<Invitation> Invitations { get; set; } = new List<Invitation>();
}
