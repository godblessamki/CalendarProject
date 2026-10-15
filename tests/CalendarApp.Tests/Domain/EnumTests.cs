using CalendarApp.Domain.Enums;

namespace CalendarApp.Tests.Domain;

public class EnumTests
{
    [Fact]
    public void RecurrenceType_ShouldContainExpectedValues()
    {
        // Arrange & Act
        var values = Enum.GetValues<RecurrenceType>();

        // Assert
        Assert.Equal(5, values.Length);
        Assert.Contains(RecurrenceType.None, values);
        Assert.Contains(RecurrenceType.Daily, values);
        Assert.Contains(RecurrenceType.Weekly, values);
        Assert.Contains(RecurrenceType.Monthly, values);
        Assert.Contains(RecurrenceType.Yearly, values);
    }

    [Fact]
    public void ShareAccessLevel_ShouldContainExpectedValues()
    {
        // Arrange & Act
        var values = Enum.GetValues<ShareAccessLevel>();

        // Assert
        Assert.Equal(2, values.Length);
        Assert.Contains(ShareAccessLevel.ReadOnly, values);
        Assert.Contains(ShareAccessLevel.ReadWrite, values);
    }

    [Fact]
    public void InvitationStatus_ShouldContainExpectedValues()
    {
        // Arrange & Act
        var values = Enum.GetValues<InvitationStatus>();

        // Assert
        Assert.Equal(3, values.Length);
        Assert.Contains(InvitationStatus.Pending, values);
        Assert.Contains(InvitationStatus.Accepted, values);
        Assert.Contains(InvitationStatus.Declined, values);
    }

    [Fact]
    public void NotificationType_ShouldContainExpectedValues()
    {
        // Arrange & Act
        var values = Enum.GetValues<NotificationType>();

        // Assert
        Assert.Equal(4, values.Length);
        Assert.Contains(NotificationType.EventReminder, values);
        Assert.Contains(NotificationType.InvitationReceived, values);
        Assert.Contains(NotificationType.CalendarShared, values);
        Assert.Contains(NotificationType.InvitationResponse, values);
    }
}
