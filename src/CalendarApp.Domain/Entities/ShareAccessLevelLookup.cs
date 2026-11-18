namespace CalendarApp.Domain.Entities;

using CalendarApp.Domain.Enums;

public class ShareAccessLevelLookup
{
    public ShareAccessLevel Id { get; set; }
    public string Name { get; set; } = string.Empty;

    public ShareAccessLevelLookup()
    {
    }

    public ShareAccessLevelLookup(ShareAccessLevel id, string name)
    {
        Id = id;
        Name = name;
    }
}
