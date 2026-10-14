namespace CalendarApp.Domain.Entities;

using Microsoft.AspNetCore.Identity;

public class ApplicationUser : IdentityUser
{
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string? ProfileImagePath { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    // Navigation properties
    public ICollection<Calendar> Calendars { get; set; } = new List<Calendar>();
    public ICollection<CalendarShare> SharedCalendars { get; set; } = new List<CalendarShare>();
    public ICollection<Notification> Notifications { get; set; } = new List<Notification>();
    public ICollection<Invitation> Invitations { get; set; } = new List<Invitation>();
}
