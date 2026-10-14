namespace CalendarApp.Domain.Entities;

public class Calendar
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public string Color { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

    // Foreign keys
    public string OwnerId { get; set; } = string.Empty;
    public ApplicationUser Owner { get; set; } = null!;

    // Navigation
    public ICollection<Event> Events { get; set; } = new List<Event>();
    public ICollection<CalendarShare> Shares { get; set; } = new List<CalendarShare>();
}
