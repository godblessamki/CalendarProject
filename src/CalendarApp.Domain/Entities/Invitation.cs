namespace CalendarApp.Domain.Entities;

using CalendarApp.Domain.Enums;

public class Invitation
{
    public int Id { get; set; }
    public InvitationStatus Status { get; set; } = InvitationStatus.Pending;
    public DateTime SentAt { get; set; } = DateTime.UtcNow;
    public DateTime? RespondedAt { get; set; }
    public string? Message { get; set; }

    // Foreign keys
    public int EventId { get; set; }
    public Event Event { get; set; } = null!;
    public string InvitedUserId { get; set; } = string.Empty;
    public ApplicationUser InvitedUser { get; set; } = null!;
    public string InvitedByUserId { get; set; } = string.Empty;
}
