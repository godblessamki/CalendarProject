namespace CalendarApp.Domain.Interfaces;

using CalendarApp.Domain.Entities;

/// <summary>
/// Repository interface for Category entity operations.
/// </summary>
public interface ICategoryRepository : IRepository<Category>
{
    Task<Category?> GetByNameAsync(string name, CancellationToken cancellationToken = default);
    Task<Category?> GetCategoryWithEventsAsync(int id, CancellationToken cancellationToken = default);
}
