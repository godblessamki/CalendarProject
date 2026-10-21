namespace CalendarApp.Infrastructure.Repositories;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using CalendarApp.Domain.Entities;
using CalendarApp.Domain.Interfaces;
using CalendarApp.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

/// <summary>
/// Repository implementation for Event entity operations.
/// </summary>
public class EventRepository : Repository<Event>, IEventRepository
{
    public EventRepository(ApplicationDbContext context) : base(context)
    {
    }

    public async Task<IReadOnlyList<Event>> GetEventsByCalendarIdAsync(int calendarId, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Where(e => e.CalendarId == calendarId)
            .OrderBy(e => e.StartDate)
            .ToListAsync(cancellationToken);
    }

    public async Task<IReadOnlyList<Event>> GetEventsByDateRangeAsync(int calendarId, DateTime startDate, DateTime endDate, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Where(e => e.CalendarId == calendarId && e.StartDate <= endDate && e.EndDate >= startDate)
            .OrderBy(e => e.StartDate)
            .ToListAsync(cancellationToken);
    }

    public async Task<IReadOnlyList<Event>> GetEventsByUserIdAsync(string userId, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Where(e => e.Calendar.OwnerId == userId)
            .OrderBy(e => e.StartDate)
            .ToListAsync(cancellationToken);
    }

    public async Task<IReadOnlyList<Event>> GetEventsByCategoryIdAsync(int categoryId, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Where(e => e.CategoryId == categoryId)
            .OrderBy(e => e.StartDate)
            .ToListAsync(cancellationToken);
    }

    public async Task<IReadOnlyList<Event>> GetUpcomingEventsAsync(DateTime fromDate, DateTime toDate, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Where(e => e.StartDate >= fromDate && e.StartDate <= toDate)
            .OrderBy(e => e.StartDate)
            .ToListAsync(cancellationToken);
    }

    public async Task<Event?> GetEventWithDetailsAsync(int id, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Include(e => e.Calendar)
                .ThenInclude(c => c.Owner)
            .Include(e => e.Category)
            .Include(e => e.Notifications)
            .Include(e => e.Invitations)
                .ThenInclude(i => i.InvitedUser)
            .FirstOrDefaultAsync(e => e.Id == id, cancellationToken);
    }
}
