namespace CalendarApp.Tests.Domain;

using CalendarApp.Domain.Entities;
using CalendarApp.Domain.Enums;

public class EntityTests
{
    [Fact]
    public void Calendar_ShouldInitializeWithDefaultValuesAndCollections()
    {
        // Arrange & Act
        var calendar = new Calendar
        {
            Id = 1,
            Name = "Work",
            Description = "Work related tasks",
            Color = "#3B82F6",
            OwnerId = "user-1"
        };

        // Assert
        Assert.Equal(1, calendar.Id);
        Assert.Equal("Work", calendar.Name);
        Assert.Equal("Work related tasks", calendar.Description);
        Assert.Equal("#3B82F6", calendar.Color);
        Assert.Equal("user-1", calendar.OwnerId);
        Assert.True(calendar.CreatedAt <= DateTime.UtcNow);
        Assert.True(calendar.UpdatedAt <= DateTime.UtcNow);
        Assert.NotNull(calendar.Events);
        Assert.Empty(calendar.Events);
        Assert.NotNull(calendar.Shares);
        Assert.Empty(calendar.Shares);
    }

    [Fact]
    public void Event_ShouldInitializeWithDefaultValuesAndCollections()
    {
        // Arrange & Act
        var calendar = new Calendar { Id = 1, Name = "Personal" };
        var category = new Category { Id = 10, Name = "Meeting", Color = "#EF4444" };
        var startDate = DateTime.UtcNow.AddHours(1);
        var endDate = startDate.AddHours(2);

        var ev = new Event
        {
            Id = 42,
            Title = "Team Standup",
            Description = "Daily standup meeting",
            Location = "Room 301",
            StartDate = startDate,
            EndDate = endDate,
            IsAllDay = false,
            Recurrence = RecurrenceType.Daily,
            RecurrenceEndDate = startDate.AddMonths(1),
            CalendarId = calendar.Id,
            Calendar = calendar,
            CategoryId = category.Id,
            Category = category
        };

        // Assert
        Assert.Equal(42, ev.Id);
        Assert.Equal("Team Standup", ev.Title);
        Assert.Equal("Daily standup meeting", ev.Description);
        Assert.Equal("Room 301", ev.Location);
        Assert.Equal(startDate, ev.StartDate);
        Assert.Equal(endDate, ev.EndDate);
        Assert.False(ev.IsAllDay);
        Assert.Equal(RecurrenceType.Daily, ev.Recurrence);
        Assert.NotNull(ev.RecurrenceEndDate);
        Assert.Equal(1, ev.CalendarId);
        Assert.Same(calendar, ev.Calendar);
        Assert.Equal(10, ev.CategoryId);
        Assert.Same(category, ev.Category);
        Assert.NotNull(ev.Notifications);
        Assert.Empty(ev.Notifications);
        Assert.NotNull(ev.Invitations);
        Assert.Empty(ev.Invitations);
    }

    [Fact]
    public void Category_ShouldInitializeWithPropertiesAndCollections()
    {
        // Arrange & Act
        var category = new Category
        {
            Id = 5,
            Name = "Lecture",
            Color = "#10B981",
            Icon = "bi-book"
        };

        // Assert
        Assert.Equal(5, category.Id);
        Assert.Equal("Lecture", category.Name);
        Assert.Equal("#10B981", category.Color);
        Assert.Equal("bi-book", category.Icon);
        Assert.NotNull(category.Events);
        Assert.Empty(category.Events);
    }

    [Fact]
    public void Notification_ShouldInitializeWithCorrectDefaultsAndProperties()
    {
        // Arrange & Act
        var ev = new Event { Id = 99, Title = "Important Meeting" };
        var remindAt = DateTime.UtcNow.AddMinutes(15);

        var notification = new Notification
        {
            Id = 7,
            Message = "Event reminder: Important Meeting starts soon",
            Type = NotificationType.EventReminder,
            RemindAt = remindAt,
            UserId = "user-2",
            EventId = ev.Id,
            Event = ev
        };

        // Assert
        Assert.Equal(7, notification.Id);
        Assert.Equal("Event reminder: Important Meeting starts soon", notification.Message);
        Assert.False(notification.IsRead);
        Assert.Null(notification.ReadAt);
        Assert.Equal(NotificationType.EventReminder, notification.Type);
        Assert.Equal(remindAt, notification.RemindAt);
        Assert.Equal("user-2", notification.UserId);
        Assert.Equal(99, notification.EventId);
        Assert.Same(ev, notification.Event);
    }

    [Fact]
    public void CalendarShare_ShouldInitializeWithDefaultsAndProperties()
    {
        // Arrange & Act
        var calendar = new Calendar { Id = 3, Name = "Shared Calendar" };

        var share = new CalendarShare
        {
            Id = 12,
            CalendarId = calendar.Id,
            Calendar = calendar,
            SharedWithUserId = "user-3",
            AccessLevel = ShareAccessLevel.ReadWrite,
            IsAccepted = true
        };

        // Assert
        Assert.Equal(12, share.Id);
        Assert.Equal(3, share.CalendarId);
        Assert.Same(calendar, share.Calendar);
        Assert.Equal("user-3", share.SharedWithUserId);
        Assert.Equal(ShareAccessLevel.ReadWrite, share.AccessLevel);
        Assert.True(share.IsAccepted);
        Assert.True(share.SharedAt <= DateTime.UtcNow);
    }

    [Fact]
    public void Invitation_ShouldInitializeWithDefaultsAndProperties()
    {
        // Arrange & Act
        var ev = new Event { Id = 88, Title = "Birthday Party" };

        var invitation = new Invitation
        {
            Id = 21,
            EventId = ev.Id,
            Event = ev,
            InvitedUserId = "user-4",
            InvitedByUserId = "user-admin",
            Message = "Please join my party!"
        };

        // Assert
        Assert.Equal(21, invitation.Id);
        Assert.Equal(InvitationStatus.Pending, invitation.Status);
        Assert.Equal(88, invitation.EventId);
        Assert.Same(ev, invitation.Event);
        Assert.Equal("user-4", invitation.InvitedUserId);
        Assert.Equal("user-admin", invitation.InvitedByUserId);
        Assert.Equal("Please join my party!", invitation.Message);
        Assert.Null(invitation.RespondedAt);
        Assert.True(invitation.SentAt <= DateTime.UtcNow);
    }

    [Fact]
    public void LookupEntities_ShouldInitializeWithProperties()
    {
        // RecurrenceTypeLookup
        var recurrence = new RecurrenceTypeLookup { Id = RecurrenceType.Daily, Name = "Daily" };
        Assert.Equal(RecurrenceType.Daily, recurrence.Id);
        Assert.Equal("Daily", recurrence.Name);

        var recurrenceCtor = new RecurrenceTypeLookup(RecurrenceType.Weekly, "Weekly");
        Assert.Equal(RecurrenceType.Weekly, recurrenceCtor.Id);
        Assert.Equal("Weekly", recurrenceCtor.Name);

        // ShareAccessLevelLookup
        var shareLevel = new ShareAccessLevelLookup { Id = ShareAccessLevel.Manage, Name = "Manage" };
        Assert.Equal(ShareAccessLevel.Manage, shareLevel.Id);
        Assert.Equal("Manage", shareLevel.Name);

        var shareLevelCtor = new ShareAccessLevelLookup(ShareAccessLevel.ReadOnly, "ReadOnly");
        Assert.Equal(ShareAccessLevel.ReadOnly, shareLevelCtor.Id);
        Assert.Equal("ReadOnly", shareLevelCtor.Name);

        // InvitationStatusLookup
        var status = new InvitationStatusLookup { Id = InvitationStatus.Tentative, Name = "Tentative" };
        Assert.Equal(InvitationStatus.Tentative, status.Id);
        Assert.Equal("Tentative", status.Name);

        var statusCtor = new InvitationStatusLookup(InvitationStatus.Accepted, "Accepted");
        Assert.Equal(InvitationStatus.Accepted, statusCtor.Id);
        Assert.Equal("Accepted", statusCtor.Name);

        // NotificationTypeLookup
        var notification = new NotificationTypeLookup { Id = NotificationType.System, Name = "System" };
        Assert.Equal(NotificationType.System, notification.Id);
        Assert.Equal("System", notification.Name);

        var notificationCtor = new NotificationTypeLookup(NotificationType.EventInvitation, "EventInvitation");
        Assert.Equal(NotificationType.EventInvitation, notificationCtor.Id);
        Assert.Equal("EventInvitation", notificationCtor.Name);
    }
}
