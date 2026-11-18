namespace CalendarApp.Domain.Entities;

using CalendarApp.Domain.Enums;

public class InvitationStatusLookup
{
    public InvitationStatus Id { get; set; }
    public string Name { get; set; } = string.Empty;

    public InvitationStatusLookup()
    {
    }

    public InvitationStatusLookup(InvitationStatus id, string name)
    {
        Id = id;
        Name = name;
    }
}
