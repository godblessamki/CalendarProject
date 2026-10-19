namespace CalendarApp.Tests.Domain;

using CalendarApp.Domain.Entities;
using CalendarApp.Domain.Interfaces;

public class RepositoryInterfaceTests
{
    [Fact]
    public void IRepository_ShouldDeclareExpectedCrudMethods()
    {
        // Arrange
        var interfaceType = typeof(IRepository<Event>);

        // Act & Assert
        Assert.NotNull(interfaceType.GetMethod("GetByIdAsync"));
        Assert.NotNull(interfaceType.GetMethod("GetAllAsync"));
        Assert.NotNull(interfaceType.GetMethod("AddAsync"));
        Assert.NotNull(interfaceType.GetMethod("Update"));
        Assert.NotNull(interfaceType.GetMethod("Delete"));
        Assert.NotNull(interfaceType.GetMethod("SaveChangesAsync"));
    }

    [Fact]
    public void IEventRepository_ShouldInheritFromGenericRepositoryAndDeclareSpecificMethods()
    {
        // Arrange
        var interfaceType = typeof(IEventRepository);

        // Act & Assert
        Assert.True(typeof(IRepository<Event>).IsAssignableFrom(interfaceType));
        Assert.NotNull(interfaceType.GetMethod("GetEventsByCalendarIdAsync"));
        Assert.NotNull(interfaceType.GetMethod("GetEventsByDateRangeAsync"));
        Assert.NotNull(interfaceType.GetMethod("GetEventsByUserIdAsync"));
        Assert.NotNull(interfaceType.GetMethod("GetEventsByCategoryIdAsync"));
        Assert.NotNull(interfaceType.GetMethod("GetUpcomingEventsAsync"));
        Assert.NotNull(interfaceType.GetMethod("GetEventWithDetailsAsync"));
    }

    [Fact]
    public void ICalendarRepository_ShouldInheritFromGenericRepositoryAndDeclareSpecificMethods()
    {
        // Arrange
        var interfaceType = typeof(ICalendarRepository);

        // Act & Assert
        Assert.True(typeof(IRepository<Calendar>).IsAssignableFrom(interfaceType));
        Assert.NotNull(interfaceType.GetMethod("GetCalendarsByUserIdAsync"));
        Assert.NotNull(interfaceType.GetMethod("GetSharedCalendarsForUserAsync"));
        Assert.NotNull(interfaceType.GetMethod("GetCalendarWithDetailsAsync"));
        Assert.NotNull(interfaceType.GetMethod("IsUserOwnerAsync"));
    }

    [Fact]
    public void ICategoryRepository_ShouldInheritFromGenericRepositoryAndDeclareSpecificMethods()
    {
        // Arrange
        var interfaceType = typeof(ICategoryRepository);

        // Act & Assert
        Assert.True(typeof(IRepository<Category>).IsAssignableFrom(interfaceType));
        Assert.NotNull(interfaceType.GetMethod("GetByNameAsync"));
        Assert.NotNull(interfaceType.GetMethod("GetCategoryWithEventsAsync"));
    }

    [Fact]
    public void INotificationRepository_ShouldInheritFromGenericRepositoryAndDeclareSpecificMethods()
    {
        // Arrange
        var interfaceType = typeof(INotificationRepository);

        // Act & Assert
        Assert.True(typeof(IRepository<Notification>).IsAssignableFrom(interfaceType));
        Assert.NotNull(interfaceType.GetMethod("GetNotificationsByUserIdAsync"));
        Assert.NotNull(interfaceType.GetMethod("GetUnreadByUserIdAsync"));
        Assert.NotNull(interfaceType.GetMethod("GetUnreadCountAsync"));
        Assert.NotNull(interfaceType.GetMethod("MarkAllAsReadAsync"));
    }

    [Fact]
    public void ICalendarShareRepository_ShouldInheritFromGenericRepositoryAndDeclareSpecificMethods()
    {
        // Arrange
        var interfaceType = typeof(ICalendarShareRepository);

        // Act & Assert
        Assert.True(typeof(IRepository<CalendarShare>).IsAssignableFrom(interfaceType));
        Assert.NotNull(interfaceType.GetMethod("GetSharesByCalendarIdAsync"));
        Assert.NotNull(interfaceType.GetMethod("GetSharesByUserIdAsync"));
        Assert.NotNull(interfaceType.GetMethod("GetShareAsync"));
        Assert.NotNull(interfaceType.GetMethod("GetAccessLevelAsync"));
    }

    [Fact]
    public void IInvitationRepository_ShouldInheritFromGenericRepositoryAndDeclareSpecificMethods()
    {
        // Arrange
        var interfaceType = typeof(IInvitationRepository);

        // Act & Assert
        Assert.True(typeof(IRepository<Invitation>).IsAssignableFrom(interfaceType));
        Assert.NotNull(interfaceType.GetMethod("GetInvitationsByEventIdAsync"));
        Assert.NotNull(interfaceType.GetMethod("GetInvitationsByUserIdAsync"));
        Assert.NotNull(interfaceType.GetMethod("GetPendingInvitationsByUserIdAsync"));
        Assert.NotNull(interfaceType.GetMethod("GetInvitationWithDetailsAsync"));
    }
}
