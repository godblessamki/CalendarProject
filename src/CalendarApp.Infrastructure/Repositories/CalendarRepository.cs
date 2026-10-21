namespace CalendarApp.Infrastructure.Repositories;

using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using CalendarApp.Domain.Entities;
using CalendarApp.Domain.Interfaces;
using CalendarApp.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

/// <summary>
/// Repository implementation for Calendar entity operations.
/// </summary>
public class CalendarRepository : Repository<Calendar>, ICalendarRepository
{
    public CalendarRepository(ApplicationDbContext context) : base(context)
    {
    }

    public async Task<IReadOnlyList<Calendar>> GetCalendarsByUserIdAsync(string userId, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Where(c => c.OwnerId == userId)
            .OrderBy(c => c.Name)
            .ToListAsync(cancellationToken);
    }

    public async Task<IReadOnlyList<Calendar>> GetSharedCalendarsForUserAsync(string userId, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Where(c => c.Shares.Any(s => s.SharedWithUserId == userId && s.IsAccepted))
            .OrderBy(c => c.Name)
            .ToListAsync(cancellationToken);
    }

    public async Task<Calendar?> GetCalendarWithDetailsAsync(int id, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Include(c => c.Owner)
            .Include(c => c.Events)
            .Include(c => c.Shares)
                .ThenInclude(s => s.SharedWithUser)
            .FirstOrDefaultAsync(c => c.Id == id, cancellationToken);
    }

    public async Task<bool> IsUserOwnerAsync(int calendarId, string userId, CancellationToken cancellationToken = default)
    {
        return await _dbSet.AnyAsync(c => c.Id == calendarId && c.OwnerId == userId, cancellationToken);
    }
}
