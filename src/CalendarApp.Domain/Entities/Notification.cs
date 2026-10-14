namespace CalendarApp.Domain.Entities;

using CalendarApp.Domain.Enums;

public class Notification
{
    public int Id { get; set; }
    public string Message { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? ReadAt { get; set; }
    public bool IsRead { get; set; }
    public NotificationType Type { get; set; } = NotificationType.EventReminder;
    public DateTime? RemindAt { get; set; }

    // Foreign keys
    public string UserId { get; set; } = string.Empty;
    public ApplicationUser User { get; set; } = null!;
    public int? EventId { get; set; }
    public Event? Event { get; set; }
}
