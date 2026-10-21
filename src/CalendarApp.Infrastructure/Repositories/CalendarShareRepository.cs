namespace CalendarApp.Infrastructure.Repositories;

using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using CalendarApp.Domain.Entities;
using CalendarApp.Domain.Enums;
using CalendarApp.Domain.Interfaces;
using CalendarApp.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

/// <summary>
/// Repository implementation for CalendarShare entity operations.
/// </summary>
public class CalendarShareRepository : Repository<CalendarShare>, ICalendarShareRepository
{
    public CalendarShareRepository(ApplicationDbContext context) : base(context)
    {
    }

    public async Task<IReadOnlyList<CalendarShare>> GetSharesByCalendarIdAsync(int calendarId, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Include(s => s.SharedWithUser)
            .Where(s => s.CalendarId == calendarId)
            .ToListAsync(cancellationToken);
    }

    public async Task<IReadOnlyList<CalendarShare>> GetSharesByUserIdAsync(string userId, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Include(s => s.Calendar)
            .Where(s => s.SharedWithUserId == userId)
            .ToListAsync(cancellationToken);
    }

    public async Task<CalendarShare?> GetShareAsync(int calendarId, string userId, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Include(s => s.Calendar)
            .Include(s => s.SharedWithUser)
            .FirstOrDefaultAsync(s => s.CalendarId == calendarId && s.SharedWithUserId == userId, cancellationToken);
    }

    public async Task<ShareAccessLevel?> GetAccessLevelAsync(int calendarId, string userId, CancellationToken cancellationToken = default)
    {
        var share = await _dbSet
            .Where(s => s.CalendarId == calendarId && s.SharedWithUserId == userId)
            .Select(s => (ShareAccessLevel?)s.AccessLevel)
            .FirstOrDefaultAsync(cancellationToken);

        return share;
    }
}
