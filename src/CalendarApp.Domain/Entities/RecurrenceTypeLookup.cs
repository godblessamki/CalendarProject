namespace CalendarApp.Domain.Entities;

using CalendarApp.Domain.Enums;

public class RecurrenceTypeLookup
{
    public RecurrenceType Id { get; set; }
    public string Name { get; set; } = string.Empty;

    public RecurrenceTypeLookup()
    {
    }

    public RecurrenceTypeLookup(RecurrenceType id, string name)
    {
        Id = id;
        Name = name;
    }
}
