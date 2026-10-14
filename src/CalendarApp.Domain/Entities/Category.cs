namespace CalendarApp.Domain.Entities;

public class Category
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Color { get; set; } = string.Empty;
    public string? Icon { get; set; }

    // Navigation
    public ICollection<Event> Events { get; set; } = new List<Event>();
}
