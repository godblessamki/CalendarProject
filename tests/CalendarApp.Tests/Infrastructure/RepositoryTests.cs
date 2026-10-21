namespace CalendarApp.Tests.Infrastructure;

using System;
using System.Linq;
using System.Threading.Tasks;
using CalendarApp.Domain.Entities;
using CalendarApp.Domain.Enums;
using CalendarApp.Domain.Interfaces;
using CalendarApp.Infrastructure.Data;
using CalendarApp.Infrastructure.Repositories;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

public class RepositoryTests : IDisposable
{
    private readonly SqliteConnection _connection;
    private readonly DbContextOptions<ApplicationDbContext> _options;

    public RepositoryTests()
    {
        _connection = new SqliteConnection("DataSource=:memory:");
        _connection.Open();

        _options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseSqlite(_connection)
            .Options;

        using var context = CreateContext();
        context.Database.EnsureCreated();
    }

    public void Dispose()
    {
        _connection.Dispose();
    }

    private ApplicationDbContext CreateContext() => new(_options);

    [Fact]
    public async Task GenericRepository_CrudOperations_ShouldWorkCorrectly()
    {
        using (var context = CreateContext())
        {
            var repo = new Repository<Category>(context);
            var category = new Category { Id = 1, Name = "Work", Color = "#0000FF" };

            var added = await repo.AddAsync(category);
            await repo.SaveChangesAsync();
            Assert.Equal("Work", added.Name);
        }

        using (var context = CreateContext())
        {
            var repo = new Repository<Category>(context);
            var retrieved = await repo.GetByIdAsync(1);
            Assert.NotNull(retrieved);
            Assert.Equal("Work", retrieved.Name);

            var all = await repo.GetAllAsync();
            Assert.Single(all);

            retrieved.Name = "Work Updated";
            repo.Update(retrieved);
            await repo.SaveChangesAsync();
        }

        using (var context = CreateContext())
        {
            var repo = new Repository<Category>(context);
            var updated = await repo.GetByIdAsync(1);
            Assert.NotNull(updated);
            Assert.Equal("Work Updated", updated.Name);

            repo.Delete(updated);
            await repo.SaveChangesAsync();
        }

        using (var context = CreateContext())
        {
            var repo = new Repository<Category>(context);
            var deleted = await repo.GetByIdAsync(1);
            Assert.Null(deleted);
            var all = await repo.GetAllAsync();
            Assert.Empty(all);
        }
    }

    [Fact]
    public async Task CalendarRepository_ShouldPerformSpecificQueries()
    {
        using (var context = CreateContext())
        {
            var user1 = new ApplicationUser { Id = "u1", UserName = "u1", FirstName = "Alice", LastName = "A" };
            var user2 = new ApplicationUser { Id = "u2", UserName = "u2", FirstName = "Bob", LastName = "B" };
            await context.Users.AddRangeAsync(user1, user2);

            var cal1 = new Calendar { Id = 1, Name = "Personal", Color = "#111", OwnerId = "u1" };
            var cal2 = new Calendar { Id = 2, Name = "Work", Color = "#222", OwnerId = "u1" };
            var cal3 = new Calendar { Id = 3, Name = "Bob's Calendar", Color = "#333", OwnerId = "u2" };
            await context.Calendars.AddRangeAsync(cal1, cal2, cal3);

            var share = new CalendarShare
            {
                Id = 1,
                CalendarId = 3,
                SharedWithUserId = "u1",
                AccessLevel = ShareAccessLevel.ReadWrite,
                IsAccepted = true
            };
            await context.CalendarShares.AddAsync(share);

            var ev = new Event
            {
                Id = 1,
                Title = "Test Event",
                CalendarId = 1,
                StartDate = DateTime.UtcNow,
                EndDate = DateTime.UtcNow.AddHours(1)
            };
            await context.Events.AddAsync(ev);
            await context.SaveChangesAsync();
        }

        using (var context = CreateContext())
        {
            var repo = new CalendarRepository(context);

            var user1Cals = await repo.GetCalendarsByUserIdAsync("u1");
            Assert.Equal(2, user1Cals.Count);
            Assert.Contains(user1Cals, c => c.Name == "Personal");
            Assert.Contains(user1Cals, c => c.Name == "Work");

            var sharedWithUser1 = await repo.GetSharedCalendarsForUserAsync("u1");
            Assert.Single(sharedWithUser1);
            Assert.Equal("Bob's Calendar", sharedWithUser1[0].Name);

            var details = await repo.GetCalendarWithDetailsAsync(1);
            Assert.NotNull(details);
            Assert.Equal("Alice", details.Owner.FirstName);
            Assert.Single(details.Events);

            var isOwner = await repo.IsUserOwnerAsync(1, "u1");
            Assert.True(isOwner);

            var isNotOwner = await repo.IsUserOwnerAsync(1, "u2");
            Assert.False(isNotOwner);
        }
    }

    [Fact]
    public async Task EventRepository_ShouldPerformSpecificQueries()
    {
        var now = DateTime.UtcNow;
        using (var context = CreateContext())
        {
            var user = new ApplicationUser { Id = "u1", UserName = "u1", FirstName = "Alice", LastName = "A" };
            await context.Users.AddAsync(user);

            var cal = new Calendar { Id = 1, Name = "Main", Color = "#111", OwnerId = "u1" };
            await context.Calendars.AddAsync(cal);

            var cat = new Category { Id = 1, Name = "Meeting", Color = "#222" };
            await context.Categories.AddAsync(cat);

            var ev1 = new Event
            {
                Id = 1,
                Title = "Past Event",
                CalendarId = 1,
                CategoryId = 1,
                StartDate = now.AddDays(-2),
                EndDate = now.AddDays(-2).AddHours(1)
            };
            var ev2 = new Event
            {
                Id = 2,
                Title = "Current Event",
                CalendarId = 1,
                CategoryId = 1,
                StartDate = now.AddHours(1),
                EndDate = now.AddHours(2)
            };
            var ev3 = new Event
            {
                Id = 3,
                Title = "Future Event",
                CalendarId = 1,
                StartDate = now.AddDays(5),
                EndDate = now.AddDays(5).AddHours(2)
            };
            await context.Events.AddRangeAsync(ev1, ev2, ev3);
            await context.SaveChangesAsync();
        }

        using (var context = CreateContext())
        {
            var repo = new EventRepository(context);

            var calEvents = await repo.GetEventsByCalendarIdAsync(1);
            Assert.Equal(3, calEvents.Count);

            var rangeEvents = await repo.GetEventsByDateRangeAsync(1, now, now.AddDays(2));
            Assert.Single(rangeEvents);
            Assert.Equal("Current Event", rangeEvents[0].Title);

            var userEvents = await repo.GetEventsByUserIdAsync("u1");
            Assert.Equal(3, userEvents.Count);

            var catEvents = await repo.GetEventsByCategoryIdAsync(1);
            Assert.Equal(2, catEvents.Count);

            var upcoming = await repo.GetUpcomingEventsAsync(now, now.AddDays(6));
            Assert.Equal(2, upcoming.Count);

            var details = await repo.GetEventWithDetailsAsync(1);
            Assert.NotNull(details);
            Assert.Equal("Main", details.Calendar.Name);
            Assert.Equal("Meeting", details.Category?.Name);
        }
    }

    [Fact]
    public async Task CategoryRepository_ShouldPerformSpecificQueries()
    {
        using (var context = CreateContext())
        {
            var cat = new Category { Id = 1, Name = "Study", Color = "#ABCDEF" };
            await context.Categories.AddAsync(cat);

            var user = new ApplicationUser { Id = "u1", UserName = "u1", FirstName = "A", LastName = "B" };
            await context.Users.AddAsync(user);

            var cal = new Calendar { Id = 1, Name = "Cal", Color = "#000", OwnerId = "u1" };
            await context.Calendars.AddAsync(cal);

            var ev = new Event
            {
                Id = 1,
                Title = "Study Session",
                CalendarId = 1,
                CategoryId = 1,
                StartDate = DateTime.UtcNow,
                EndDate = DateTime.UtcNow.AddHours(1)
            };
            await context.Events.AddAsync(ev);
            await context.SaveChangesAsync();
        }

        using (var context = CreateContext())
        {
            var repo = new CategoryRepository(context);

            var byName = await repo.GetByNameAsync("Study");
            Assert.NotNull(byName);
            Assert.Equal("#ABCDEF", byName.Color);

            var nonExistent = await repo.GetByNameAsync("NonExistent");
            Assert.Null(nonExistent);

            var withEvents = await repo.GetCategoryWithEventsAsync(1);
            Assert.NotNull(withEvents);
            Assert.Single(withEvents.Events);
            Assert.Equal("Study Session", withEvents.Events.First().Title);
        }
    }

    [Fact]
    public async Task NotificationRepository_ShouldPerformSpecificQueries()
    {
        using (var context = CreateContext())
        {
            var user = new ApplicationUser { Id = "u1", UserName = "u1", FirstName = "Alice", LastName = "A" };
            await context.Users.AddAsync(user);

            var n1 = new Notification { Id = 1, UserId = "u1", Message = "Msg 1", IsRead = false, CreatedAt = DateTime.UtcNow.AddMinutes(-10) };
            var n2 = new Notification { Id = 2, UserId = "u1", Message = "Msg 2", IsRead = true, ReadAt = DateTime.UtcNow, CreatedAt = DateTime.UtcNow.AddMinutes(-5) };
            var n3 = new Notification { Id = 3, UserId = "u1", Message = "Msg 3", IsRead = false, CreatedAt = DateTime.UtcNow };
            await context.Notifications.AddRangeAsync(n1, n2, n3);
            await context.SaveChangesAsync();
        }

        using (var context = CreateContext())
        {
            var repo = new NotificationRepository(context);

            var all = await repo.GetNotificationsByUserIdAsync("u1");
            Assert.Equal(3, all.Count);

            var unread = await repo.GetUnreadByUserIdAsync("u1");
            Assert.Equal(2, unread.Count);

            var count = await repo.GetUnreadCountAsync("u1");
            Assert.Equal(2, count);

            await repo.MarkAllAsReadAsync("u1");
        }

        using (var context = CreateContext())
        {
            var repo = new NotificationRepository(context);
            var unread = await repo.GetUnreadByUserIdAsync("u1");
            Assert.Empty(unread);
            var count = await repo.GetUnreadCountAsync("u1");
            Assert.Equal(0, count);
        }
    }

    [Fact]
    public async Task CalendarShareRepository_ShouldPerformSpecificQueries()
    {
        using (var context = CreateContext())
        {
            var owner = new ApplicationUser { Id = "u1", UserName = "u1", FirstName = "Owner", LastName = "O" };
            var user = new ApplicationUser { Id = "u2", UserName = "u2", FirstName = "User", LastName = "U" };
            await context.Users.AddRangeAsync(owner, user);

            var cal = new Calendar { Id = 1, Name = "Shared", Color = "#000", OwnerId = "u1" };
            await context.Calendars.AddAsync(cal);

            var share = new CalendarShare
            {
                Id = 1,
                CalendarId = 1,
                SharedWithUserId = "u2",
                AccessLevel = ShareAccessLevel.ReadWrite,
                IsAccepted = true
            };
            await context.CalendarShares.AddAsync(share);
            await context.SaveChangesAsync();
        }

        using (var context = CreateContext())
        {
            var repo = new CalendarShareRepository(context);

            var byCal = await repo.GetSharesByCalendarIdAsync(1);
            Assert.Single(byCal);
            Assert.Equal("User", byCal[0].SharedWithUser.FirstName);

            var byUser = await repo.GetSharesByUserIdAsync("u2");
            Assert.Single(byUser);
            Assert.Equal("Shared", byUser[0].Calendar.Name);

            var share = await repo.GetShareAsync(1, "u2");
            Assert.NotNull(share);
            Assert.Equal(ShareAccessLevel.ReadWrite, share.AccessLevel);

            var accessLevel = await repo.GetAccessLevelAsync(1, "u2");
            Assert.Equal(ShareAccessLevel.ReadWrite, accessLevel);

            var noAccess = await repo.GetAccessLevelAsync(1, "unknown");
            Assert.Null(noAccess);
        }
    }

    [Fact]
    public async Task InvitationRepository_ShouldPerformSpecificQueries()
    {
        using (var context = CreateContext())
        {
            var inviter = new ApplicationUser { Id = "u1", UserName = "u1", FirstName = "Inviter", LastName = "I" };
            var invitee = new ApplicationUser { Id = "u2", UserName = "u2", FirstName = "Invitee", LastName = "I" };
            await context.Users.AddRangeAsync(inviter, invitee);

            var cal = new Calendar { Id = 1, Name = "Cal", Color = "#000", OwnerId = "u1" };
            await context.Calendars.AddAsync(cal);

            var ev = new Event { Id = 1, Title = "Party", CalendarId = 1, StartDate = DateTime.UtcNow, EndDate = DateTime.UtcNow.AddHours(2) };
            await context.Events.AddAsync(ev);

            var inv1 = new Invitation
            {
                Id = 1,
                EventId = 1,
                InvitedUserId = "u2",
                InvitedByUserId = "u1",
                Status = InvitationStatus.Pending,
                Message = "Join us!"
            };
            await context.Invitations.AddAsync(inv1);
            await context.SaveChangesAsync();
        }

        using (var context = CreateContext())
        {
            var repo = new InvitationRepository(context);

            var byEvent = await repo.GetInvitationsByEventIdAsync(1);
            Assert.Single(byEvent);
            Assert.Equal("Invitee", byEvent[0].InvitedUser.FirstName);

            var byUser = await repo.GetInvitationsByUserIdAsync("u2");
            Assert.Single(byUser);
            Assert.Equal("Party", byUser[0].Event.Title);

            var pending = await repo.GetPendingInvitationsByUserIdAsync("u2");
            Assert.Single(pending);

            var details = await repo.GetInvitationWithDetailsAsync(1);
            Assert.NotNull(details);
            Assert.Equal("Party", details.Event.Title);
            Assert.Equal("Cal", details.Event.Calendar.Name);
            Assert.Equal("Invitee", details.InvitedUser.FirstName);
        }
    }

    [Fact]
    public void DependencyInjection_ShouldResolveAllRepositoryInterfaces()
    {
        var services = new Microsoft.Extensions.DependencyInjection.ServiceCollection();

        services.AddDbContext<ApplicationDbContext>(options =>
            options.UseSqlite(_connection));

        services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
        services.AddScoped<IEventRepository, EventRepository>();
        services.AddScoped<ICalendarRepository, CalendarRepository>();
        services.AddScoped<ICategoryRepository, CategoryRepository>();
        services.AddScoped<INotificationRepository, NotificationRepository>();
        services.AddScoped<ICalendarShareRepository, CalendarShareRepository>();
        services.AddScoped<IInvitationRepository, InvitationRepository>();

        using var provider = services.BuildServiceProvider();

        Assert.NotNull(provider.GetService<IRepository<Event>>());
        Assert.NotNull(provider.GetService<IEventRepository>());
        Assert.NotNull(provider.GetService<ICalendarRepository>());
        Assert.NotNull(provider.GetService<ICategoryRepository>());
        Assert.NotNull(provider.GetService<INotificationRepository>());
        Assert.NotNull(provider.GetService<ICalendarShareRepository>());
        Assert.NotNull(provider.GetService<IInvitationRepository>());
    }
}
