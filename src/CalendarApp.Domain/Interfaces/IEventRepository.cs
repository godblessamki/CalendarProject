namespace CalendarApp.Domain.Interfaces;

using CalendarApp.Domain.Entities;

/// <summary>
/// Repository interface for Event entity operations.
/// </summary>
public interface IEventRepository : IRepository<Event>
{
    Task<IReadOnlyList<Event>> GetEventsByCalendarIdAsync(int calendarId, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<Event>> GetEventsByDateRangeAsync(int calendarId, DateTime startDate, DateTime endDate, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<Event>> GetEventsByUserIdAsync(string userId, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<Event>> GetEventsByCategoryIdAsync(int categoryId, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<Event>> GetUpcomingEventsAsync(DateTime fromDate, DateTime toDate, CancellationToken cancellationToken = default);
    Task<Event?> GetEventWithDetailsAsync(int id, CancellationToken cancellationToken = default);
}
