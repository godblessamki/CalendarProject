namespace CalendarApp.Domain.Interfaces;

using CalendarApp.Domain.Entities;
using CalendarApp.Domain.Enums;

/// <summary>
/// Repository interface for CalendarShare entity operations.
/// </summary>
public interface ICalendarShareRepository : IRepository<CalendarShare>
{
    Task<IReadOnlyList<CalendarShare>> GetSharesByCalendarIdAsync(int calendarId, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<CalendarShare>> GetSharesByUserIdAsync(string userId, CancellationToken cancellationToken = default);
    Task<CalendarShare?> GetShareAsync(int calendarId, string userId, CancellationToken cancellationToken = default);
    Task<ShareAccessLevel?> GetAccessLevelAsync(int calendarId, string userId, CancellationToken cancellationToken = default);
}
