namespace CalendarApp.Tests.Infrastructure;

using System;
using CalendarApp.Infrastructure.Identity;
using Microsoft.AspNetCore.Identity;
using Xunit;

public class ApplicationUserTests
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
}
