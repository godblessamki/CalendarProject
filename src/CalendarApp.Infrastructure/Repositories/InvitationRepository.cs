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
/// Repository implementation for Invitation entity operations.
/// </summary>
public class InvitationRepository : Repository<Invitation>, IInvitationRepository
{
    public InvitationRepository(ApplicationDbContext context) : base(context)
    {
    }

    public async Task<IReadOnlyList<Invitation>> GetInvitationsByEventIdAsync(int eventId, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Include(i => i.InvitedUser)
            .Where(i => i.EventId == eventId)
            .ToListAsync(cancellationToken);
    }

    public async Task<IReadOnlyList<Invitation>> GetInvitationsByUserIdAsync(string userId, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Include(i => i.Event)
            .Where(i => i.InvitedUserId == userId)
            .OrderByDescending(i => i.SentAt)
            .ToListAsync(cancellationToken);
    }

    public async Task<IReadOnlyList<Invitation>> GetPendingInvitationsByUserIdAsync(string userId, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Include(i => i.Event)
            .Where(i => i.InvitedUserId == userId && i.Status == InvitationStatus.Pending)
            .OrderByDescending(i => i.SentAt)
            .ToListAsync(cancellationToken);
    }

    public async Task<Invitation?> GetInvitationWithDetailsAsync(int id, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Include(i => i.Event)
                .ThenInclude(e => e.Calendar)
            .Include(i => i.InvitedUser)
            .FirstOrDefaultAsync(i => i.Id == id, cancellationToken);
    }
}
