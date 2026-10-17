namespace CalendarApp.Domain.Interfaces;

using CalendarApp.Domain.Entities;

/// <summary>
/// Repository interface for Calendar entity operations.
/// </summary>
public interface ICalendarRepository : IRepository<Calendar>
{
    Task<IReadOnlyList<Calendar>> GetCalendarsByUserIdAsync(string userId, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<Calendar>> GetSharedCalendarsForUserAsync(string userId, CancellationToken cancellationToken = default);
    Task<Calendar?> GetCalendarWithDetailsAsync(int id, CancellationToken cancellationToken = default);
    Task<bool> IsUserOwnerAsync(int calendarId, string userId, CancellationToken cancellationToken = default);
}
