using CalendarApp.Domain.Entities;
using CalendarApp.Domain.Enums;
using Microsoft.AspNetCore.Identity;

namespace CalendarApp.Tests.Domain;

public class EntityTests
{
    [Fact]
    public void ApplicationUser_ShouldInitializeCollectionsAndInheritFromIdentityUser()
    {
        // Arrange & Act
        var user = new ApplicationUser
        {
            FirstName = "John",
            LastName = "Doe",
            ProfileImagePath = "/uploads/profiles/johndoe.png"
        };

        // Assert
        Assert.IsAssignableFrom<IdentityUser>(user);
        Assert.Equal("John", user.FirstName);
        Assert.Equal("Doe", user.LastName);
        Assert.Equal("/uploads/profiles/johndoe.png", user.ProfileImagePath);
        Assert.True(user.CreatedAt <= DateTime.UtcNow);
        Assert.NotNull(user.Calendars);
        Assert.Empty(user.Calendars);
        Assert.NotNull(user.SharedCalendars);
        Assert.Empty(user.SharedCalendars);
        Assert.NotNull(user.Notifications);
        Assert.Empty(user.Notifications);
        Assert.NotNull(user.Invitations);
        Assert.Empty(user.Invitations);
    }

    [Fact]
    public void Calendar_ShouldInitializeWithDefaultValuesAndCollections()
    {
        // Arrange & Act
        var owner = new ApplicationUser { Id = "user-1", UserName = "john" };
        var calendar = new Calendar
        {
            Id = 1,
            Name = "Work",
            Description = "Work related tasks",
            Color = "#3B82F6",
            OwnerId = owner.Id,
            Owner = owner
        };

        // Assert
        Assert.Equal(1, calendar.Id);
        Assert.Equal("Work", calendar.Name);
        Assert.Equal("Work related tasks", calendar.Description);
        Assert.Equal("#3B82F6", calendar.Color);
        Assert.Equal("user-1", calendar.OwnerId);
        Assert.Same(owner, calendar.Owner);
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
        var user = new ApplicationUser { Id = "user-2" };
        var ev = new Event { Id = 99, Title = "Important Meeting" };
        var remindAt = DateTime.UtcNow.AddMinutes(15);

        var notification = new Notification
        {
            Id = 7,
            Message = "Event reminder: Important Meeting starts soon",
            Type = NotificationType.EventReminder,
            RemindAt = remindAt,
            UserId = user.Id,
            User = user,
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
        Assert.Same(user, notification.User);
        Assert.Equal(99, notification.EventId);
        Assert.Same(ev, notification.Event);
    }

    [Fact]
    public void CalendarShare_ShouldInitializeWithDefaultsAndProperties()
    {
        // Arrange & Act
        var calendar = new Calendar { Id = 3, Name = "Shared Calendar" };
        var user = new ApplicationUser { Id = "user-3" };

        var share = new CalendarShare
        {
            Id = 12,
            CalendarId = calendar.Id,
            Calendar = calendar,
            SharedWithUserId = user.Id,
            SharedWithUser = user,
            AccessLevel = ShareAccessLevel.ReadWrite,
            IsAccepted = true
        };

        // Assert
        Assert.Equal(12, share.Id);
        Assert.Equal(3, share.CalendarId);
        Assert.Same(calendar, share.Calendar);
        Assert.Equal("user-3", share.SharedWithUserId);
        Assert.Same(user, share.SharedWithUser);
        Assert.Equal(ShareAccessLevel.ReadWrite, share.AccessLevel);
        Assert.True(share.IsAccepted);
        Assert.True(share.SharedAt <= DateTime.UtcNow);
    }

    [Fact]
    public void Invitation_ShouldInitializeWithDefaultsAndProperties()
    {
        // Arrange & Act
        var ev = new Event { Id = 88, Title = "Birthday Party" };
        var invitedUser = new ApplicationUser { Id = "user-4" };

        var invitation = new Invitation
        {
            Id = 21,
            EventId = ev.Id,
            Event = ev,
            InvitedUserId = invitedUser.Id,
            InvitedUser = invitedUser,
            InvitedByUserId = "user-admin",
            Message = "Please join my party!"
        };

        // Assert
        Assert.Equal(21, invitation.Id);
        Assert.Equal(InvitationStatus.Pending, invitation.Status);
        Assert.Equal(88, invitation.EventId);
        Assert.Same(ev, invitation.Event);
        Assert.Equal("user-4", invitation.InvitedUserId);
        Assert.Same(invitedUser, invitation.InvitedUser);
        Assert.Equal("user-admin", invitation.InvitedByUserId);
        Assert.Equal("Please join my party!", invitation.Message);
        Assert.Null(invitation.RespondedAt);
        Assert.True(invitation.SentAt <= DateTime.UtcNow);
    }
}
