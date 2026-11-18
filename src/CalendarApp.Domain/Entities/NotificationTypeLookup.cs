namespace CalendarApp.Domain.Entities;

using CalendarApp.Domain.Enums;

public class NotificationTypeLookup
{
    public NotificationType Id { get; set; }
    public string Name { get; set; } = string.Empty;

    public NotificationTypeLookup()
    {
    }

    public NotificationTypeLookup(NotificationType id, string name)
    {
        Id = id;
        Name = name;
    }
}
