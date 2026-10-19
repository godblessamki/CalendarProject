namespace CalendarApp.Domain.Interfaces;

using CalendarApp.Domain.Entities;

/// <summary>
/// Repository interface for Notification entity operations.
/// </summary>
public interface INotificationRepository : IRepository<Notification>
{
    Task<IReadOnlyList<Notification>> GetNotificationsByUserIdAsync(string userId, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<Notification>> GetUnreadByUserIdAsync(string userId, CancellationToken cancellationToken = default);
    Task<int> GetUnreadCountAsync(string userId, CancellationToken cancellationToken = default);
    Task MarkAllAsReadAsync(string userId, CancellationToken cancellationToken = default);
}
