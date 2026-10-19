namespace CalendarApp.Domain.Interfaces;

using CalendarApp.Domain.Entities;

/// <summary>
/// Repository interface for Invitation entity operations.
/// </summary>
public interface IInvitationRepository : IRepository<Invitation>
{
    Task<IReadOnlyList<Invitation>> GetInvitationsByEventIdAsync(int eventId, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<Invitation>> GetInvitationsByUserIdAsync(string userId, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<Invitation>> GetPendingInvitationsByUserIdAsync(string userId, CancellationToken cancellationToken = default);
    Task<Invitation?> GetInvitationWithDetailsAsync(int id, CancellationToken cancellationToken = default);
}
