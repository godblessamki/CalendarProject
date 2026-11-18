namespace CalendarApp.Tests.Infrastructure;

using CalendarApp.Domain.Entities;
using CalendarApp.Domain.Enums;
using CalendarApp.Infrastructure.Data;
using CalendarApp.Infrastructure.Identity;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;

public class ApplicationDbContextTests : IDisposable
{
    private readonly SqliteConnection _connection;
    private readonly DbContextOptions<ApplicationDbContext> _options;

    public ApplicationDbContextTests()
    {
        _connection = new SqliteConnection("DataSource=:memory:");
        _connection.Open();

        _options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseSqlite(_connection)
            .Options;
    }

    public void Dispose()
    {
        _connection.Dispose();
    }

    private ApplicationDbContext CreateContext() => new(_options);

    [Fact]
    public void ApplicationDbContext_ShouldExposeAllRequiredDbSets()
    {
        using var context = CreateContext();

        Assert.NotNull(context.Calendars);
        Assert.NotNull(context.Events);
        Assert.NotNull(context.Categories);
        Assert.NotNull(context.Notifications);
        Assert.NotNull(context.CalendarShares);
        Assert.NotNull(context.Invitations);
        Assert.NotNull(context.Users);
        Assert.NotNull(context.RecurrenceTypes);
        Assert.NotNull(context.ShareAccessLevels);
        Assert.NotNull(context.InvitationStatuses);
        Assert.NotNull(context.NotificationTypes);
    }

    [Fact]
    public void ApplicationDbContextFactory_ShouldCreateDbContextWithSqlServerProvider()
    {
        var factory = new ApplicationDbContextFactory();
        using var context = factory.CreateDbContext(Array.Empty<string>());

        Assert.NotNull(context);
        Assert.Equal("Microsoft.EntityFrameworkCore.SqlServer", context.Database.ProviderName);
    }

    [Fact]
    public void ApplicationDbContext_ModelConfigurations_ShouldMatchExpectedMetadata()
    {
        using var context = CreateContext();
        var model = context.Model;

        // Calendar entity metadata
        var calendarEntity = model.FindEntityType(typeof(Calendar));
        Assert.NotNull(calendarEntity);
        var nameProp = calendarEntity.FindProperty(nameof(Calendar.Name));
        Assert.NotNull(nameProp);
        Assert.False(nameProp.IsNullable);
        Assert.Equal(100, nameProp.GetMaxLength());

        var colorProp = calendarEntity.FindProperty(nameof(Calendar.Color));
        Assert.NotNull(colorProp);
        Assert.False(colorProp.IsNullable);
        Assert.Equal(20, colorProp.GetMaxLength());

        // Event entity metadata
        var eventEntity = model.FindEntityType(typeof(Event));
        Assert.NotNull(eventEntity);
        var titleProp = eventEntity.FindProperty(nameof(Event.Title));
        Assert.NotNull(titleProp);
        Assert.False(titleProp.IsNullable);
        Assert.Equal(200, titleProp.GetMaxLength());

        var locProp = eventEntity.FindProperty(nameof(Event.Location));
        Assert.NotNull(locProp);
        Assert.Equal(255, locProp.GetMaxLength());

        // Category entity metadata
        var categoryEntity = model.FindEntityType(typeof(Category));
        Assert.NotNull(categoryEntity);
        var catNameProp = categoryEntity.FindProperty(nameof(Category.Name));
        Assert.NotNull(catNameProp);
        Assert.Equal(50, catNameProp.GetMaxLength());

        // RecurrenceTypeLookup entity metadata
        var recurrenceEntity = model.FindEntityType(typeof(RecurrenceTypeLookup));
        Assert.NotNull(recurrenceEntity);
        var recNameProp = recurrenceEntity.FindProperty(nameof(RecurrenceTypeLookup.Name));
        Assert.NotNull(recNameProp);
        Assert.False(recNameProp.IsNullable);
        Assert.Equal(50, recNameProp.GetMaxLength());

        // ShareAccessLevelLookup entity metadata
        var shareLevelEntity = model.FindEntityType(typeof(ShareAccessLevelLookup));
        Assert.NotNull(shareLevelEntity);
        var shareLevelNameProp = shareLevelEntity.FindProperty(nameof(ShareAccessLevelLookup.Name));
        Assert.NotNull(shareLevelNameProp);
        Assert.False(shareLevelNameProp.IsNullable);
        Assert.Equal(50, shareLevelNameProp.GetMaxLength());

        // InvitationStatusLookup entity metadata
        var invStatusEntity = model.FindEntityType(typeof(InvitationStatusLookup));
        Assert.NotNull(invStatusEntity);
        var invStatusNameProp = invStatusEntity.FindProperty(nameof(InvitationStatusLookup.Name));
        Assert.NotNull(invStatusNameProp);
        Assert.False(invStatusNameProp.IsNullable);
        Assert.Equal(50, invStatusNameProp.GetMaxLength());

        // NotificationTypeLookup entity metadata
        var notifTypeEntity = model.FindEntityType(typeof(NotificationTypeLookup));
        Assert.NotNull(notifTypeEntity);
        var notifTypeNameProp = notifTypeEntity.FindProperty(nameof(NotificationTypeLookup.Name));
        Assert.NotNull(notifTypeNameProp);
        Assert.False(notifTypeNameProp.IsNullable);
        Assert.Equal(50, notifTypeNameProp.GetMaxLength());

        // Unique indexes
        var shareEntity = model.FindEntityType(typeof(CalendarShare));
        Assert.NotNull(shareEntity);
        var shareIndex = shareEntity.GetIndexes()
            .FirstOrDefault(i => i.Properties.Select(p => p.Name).SequenceEqual(new[] { nameof(CalendarShare.CalendarId), nameof(CalendarShare.SharedWithUserId) }));
        Assert.NotNull(shareIndex);
        Assert.True(shareIndex.IsUnique);

        var invitationEntity = model.FindEntityType(typeof(Invitation));
        Assert.NotNull(invitationEntity);
        var invitationIndex = invitationEntity.GetIndexes()
            .FirstOrDefault(i => i.Properties.Select(p => p.Name).SequenceEqual(new[] { nameof(Invitation.EventId), nameof(Invitation.InvitedUserId) }));
        Assert.NotNull(invitationIndex);
        Assert.True(invitationIndex.IsUnique);
    }

    [Fact]
    public async Task ApplicationDbContext_ShouldPersistAndRetrieveAllEntitiesWithRelationships()
    {
        using (var context = CreateContext())
        {
            await context.Database.EnsureCreatedAsync();

            var user1 = new ApplicationUser
            {
                Id = "user-1",
                UserName = "alice@example.com",
                Email = "alice@example.com",
                FirstName = "Alice",
                LastName = "Smith"
            };

            var user2 = new ApplicationUser
            {
                Id = "user-2",
                UserName = "bob@example.com",
                Email = "bob@example.com",
                FirstName = "Bob",
                LastName = "Jones"
            };

            await context.Users.AddRangeAsync(user1, user2);

            var calendar = new Calendar
            {
                Id = 1,
                Name = "Work Calendar",
                Color = "#3B82F6",
                OwnerId = user1.Id
            };
            await context.Calendars.AddAsync(calendar);

            var category = new Category
            {
                Id = 1,
                Name = "Important",
                Color = "#EF4444",
                Icon = "bi-star"
            };
            await context.Categories.AddAsync(category);

            var ev = new Event
            {
                Id = 1,
                Title = "Sprint Planning",
                StartDate = DateTime.UtcNow.AddDays(1),
                EndDate = DateTime.UtcNow.AddDays(1).AddHours(2),
                CalendarId = calendar.Id,
                CategoryId = category.Id,
                Recurrence = RecurrenceType.Weekly
            };
            await context.Events.AddAsync(ev);

            var share = new CalendarShare
            {
                Id = 1,
                CalendarId = calendar.Id,
                SharedWithUserId = user2.Id,
                AccessLevel = ShareAccessLevel.ReadOnly,
                IsAccepted = true
            };
            await context.CalendarShares.AddAsync(share);

            var invitation = new Invitation
            {
                Id = 1,
                EventId = ev.Id,
                InvitedUserId = user2.Id,
                InvitedByUserId = user1.Id,
                Status = InvitationStatus.Pending
            };
            await context.Invitations.AddAsync(invitation);

            var notification = new Notification
            {
                Id = 1,
                UserId = user2.Id,
                EventId = ev.Id,
                Message = "You were invited to Sprint Planning",
                Type = NotificationType.EventInvitation
            };
            await context.Notifications.AddAsync(notification);

            await context.SaveChangesAsync();
        }

        using (var context = CreateContext())
        {
            var savedCalendar = await context.Calendars
                .Include(c => c.Events)
                .ThenInclude(e => e.Category)
                .Include(c => c.Shares)
                .FirstOrDefaultAsync(c => c.Id == 1);

            Assert.NotNull(savedCalendar);
            Assert.Equal("Work Calendar", savedCalendar.Name);
            Assert.Equal("user-1", savedCalendar.OwnerId);
            Assert.Single(savedCalendar.Events);
            Assert.Equal("Sprint Planning", savedCalendar.Events.First().Title);
            Assert.Equal("Important", savedCalendar.Events.First().Category?.Name);
            Assert.Single(savedCalendar.Shares);
            Assert.Equal("user-2", savedCalendar.Shares.First().SharedWithUserId);

            var savedInvitation = await context.Invitations
                .Include(i => i.Event)
                .FirstOrDefaultAsync(i => i.Id == 1);

            Assert.NotNull(savedInvitation);
            Assert.Equal(InvitationStatus.Pending, savedInvitation.Status);
            Assert.Equal("Sprint Planning", savedInvitation.Event.Title);
            Assert.Equal("user-2", savedInvitation.InvitedUserId);

            var savedNotification = await context.Notifications
                .Include(n => n.Event)
                .FirstOrDefaultAsync(n => n.Id == 1);

            Assert.NotNull(savedNotification);
            Assert.Equal("You were invited to Sprint Planning", savedNotification.Message);
            Assert.Equal("user-2", savedNotification.UserId);

            var userWithNav = await context.Users
                .Include(u => u.Calendars)
                .Include(u => u.SharedCalendars)
                .Include(u => u.Notifications)
                .Include(u => u.Invitations)
                .FirstOrDefaultAsync(u => u.Id == "user-2");

            Assert.NotNull(userWithNav);
            Assert.Single(userWithNav.SharedCalendars);
            Assert.Single(userWithNav.Notifications);
            Assert.Single(userWithNav.Invitations);
        }
    }

    [Fact]
    public async Task ApplicationDbContext_DeletingCategory_ShouldSetEventCategoryIdToNull()
    {
        using (var context = CreateContext())
        {
            await context.Database.EnsureCreatedAsync();

            var user = new ApplicationUser
            {
                Id = "user-1",
                UserName = "test@example.com",
                FirstName = "Test",
                LastName = "User"
            };
            await context.Users.AddAsync(user);

            var calendar = new Calendar { Id = 1, Name = "Cal", Color = "#000000", OwnerId = user.Id };
            await context.Calendars.AddAsync(calendar);

            var category = new Category { Id = 10, Name = "ToDelete", Color = "#123456" };
            await context.Categories.AddAsync(category);

            var ev = new Event
            {
                Id = 1,
                Title = "Event With Category",
                StartDate = DateTime.UtcNow,
                EndDate = DateTime.UtcNow.AddHours(1),
                CalendarId = 1,
                CategoryId = 10
            };
            await context.Events.AddAsync(ev);
            await context.SaveChangesAsync();
        }

        using (var context = CreateContext())
        {
            var category = await context.Categories.FindAsync(10);
            Assert.NotNull(category);
            context.Categories.Remove(category);
            await context.SaveChangesAsync();
        }

        using (var context = CreateContext())
        {
            var ev = await context.Events.FindAsync(1);
            Assert.NotNull(ev);
            Assert.Null(ev.CategoryId);
        }
    }

    [Fact]
    public async Task ApplicationDbContext_DeletingCalendar_ShouldCascadeDeleteEvents()
    {
        using (var context = CreateContext())
        {
            await context.Database.EnsureCreatedAsync();

            var user = new ApplicationUser
            {
                Id = "user-1",
                UserName = "test@example.com",
                FirstName = "Test",
                LastName = "User"
            };
            await context.Users.AddAsync(user);

            var calendar = new Calendar { Id = 1, Name = "Cal To Delete", Color = "#000000", OwnerId = user.Id };
            await context.Calendars.AddAsync(calendar);

            var ev = new Event
            {
                Id = 1,
                Title = "Child Event",
                StartDate = DateTime.UtcNow,
                EndDate = DateTime.UtcNow.AddHours(1),
                CalendarId = 1
            };
            await context.Events.AddAsync(ev);
            await context.SaveChangesAsync();
        }

        using (var context = CreateContext())
        {
            var calendar = await context.Calendars.FindAsync(1);
            Assert.NotNull(calendar);
            context.Calendars.Remove(calendar);
            await context.SaveChangesAsync();
        }

        using (var context = CreateContext())
        {
            var ev = await context.Events.FindAsync(1);
            Assert.Null(ev);
        }
    }

    [Fact]
    public async Task ApplicationDbContext_EnumLookupTables_ShouldBeSeededWithAllEnumValues()
    {
        using var context = CreateContext();
        await context.Database.EnsureCreatedAsync();

        // RecurrenceType
        var recurrenceTypes = await context.RecurrenceTypes.OrderBy(r => r.Id).ToListAsync();
        Assert.Equal(Enum.GetValues<RecurrenceType>().Length, recurrenceTypes.Count);
        foreach (var value in Enum.GetValues<RecurrenceType>())
        {
            var item = recurrenceTypes.FirstOrDefault(r => r.Id == value);
            Assert.NotNull(item);
            Assert.Equal(value.ToString(), item.Name);
        }

        // ShareAccessLevel
        var shareLevels = await context.ShareAccessLevels.OrderBy(s => s.Id).ToListAsync();
        Assert.Equal(Enum.GetValues<ShareAccessLevel>().Length, shareLevels.Count);
        foreach (var value in Enum.GetValues<ShareAccessLevel>())
        {
            var item = shareLevels.FirstOrDefault(s => s.Id == value);
            Assert.NotNull(item);
            Assert.Equal(value.ToString(), item.Name);
        }

        // InvitationStatus
        var invitationStatuses = await context.InvitationStatuses.OrderBy(i => i.Id).ToListAsync();
        Assert.Equal(Enum.GetValues<InvitationStatus>().Length, invitationStatuses.Count);
        foreach (var value in Enum.GetValues<InvitationStatus>())
        {
            var item = invitationStatuses.FirstOrDefault(i => i.Id == value);
            Assert.NotNull(item);
            Assert.Equal(value.ToString(), item.Name);
        }

        // NotificationType
        var notificationTypes = await context.NotificationTypes.OrderBy(n => n.Id).ToListAsync();
        Assert.Equal(Enum.GetValues<NotificationType>().Length, notificationTypes.Count);
        foreach (var value in Enum.GetValues<NotificationType>())
        {
            var item = notificationTypes.FirstOrDefault(n => n.Id == value);
            Assert.NotNull(item);
            Assert.Equal(value.ToString(), item.Name);
        }
    }
}
