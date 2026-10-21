namespace CalendarApp.Infrastructure.Repositories;

using System.Threading;
using System.Threading.Tasks;
using CalendarApp.Domain.Entities;
using CalendarApp.Domain.Interfaces;
using CalendarApp.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

/// <summary>
/// Repository implementation for Category entity operations.
/// </summary>
public class CategoryRepository : Repository<Category>, ICategoryRepository
{
    public CategoryRepository(ApplicationDbContext context) : base(context)
    {
    }

    public async Task<Category?> GetByNameAsync(string name, CancellationToken cancellationToken = default)
    {
        return await _dbSet.FirstOrDefaultAsync(c => c.Name == name, cancellationToken);
    }

    public async Task<Category?> GetCategoryWithEventsAsync(int id, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Include(c => c.Events)
            .FirstOrDefaultAsync(c => c.Id == id, cancellationToken);
    }
}
