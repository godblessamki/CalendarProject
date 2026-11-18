namespace CalendarApp.Infrastructure.Data;

using CalendarApp.Domain.Entities;
using CalendarApp.Domain.Enums;
using CalendarApp.Infrastructure.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    public DbSet<Calendar> Calendars => Set<Calendar>();
    public DbSet<Event> Events => Set<Event>();
    public DbSet<Category> Categories => Set<Category>();
    public DbSet<Notification> Notifications => Set<Notification>();
    public DbSet<CalendarShare> CalendarShares => Set<CalendarShare>();
    public DbSet<Invitation> Invitations => Set<Invitation>();
    public DbSet<RecurrenceTypeLookup> RecurrenceTypes => Set<RecurrenceTypeLookup>();
    public DbSet<ShareAccessLevelLookup> ShareAccessLevels => Set<ShareAccessLevelLookup>();
    public DbSet<InvitationStatusLookup> InvitationStatuses => Set<InvitationStatusLookup>();
    public DbSet<NotificationTypeLookup> NotificationTypes => Set<NotificationTypeLookup>();

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        builder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);

        builder.Entity<RecurrenceTypeLookup>().HasData(
            Enum.GetValues<RecurrenceType>().Select(e => new RecurrenceTypeLookup
            {
                Id = e,
                Name = e.ToString()
            })
        );

        builder.Entity<ShareAccessLevelLookup>().HasData(
            Enum.GetValues<ShareAccessLevel>().Select(e => new ShareAccessLevelLookup
            {
                Id = e,
                Name = e.ToString()
            })
        );

        builder.Entity<InvitationStatusLookup>().HasData(
            Enum.GetValues<InvitationStatus>().Select(e => new InvitationStatusLookup
            {
                Id = e,
                Name = e.ToString()
            })
        );

        builder.Entity<NotificationTypeLookup>().HasData(
            Enum.GetValues<NotificationType>().Select(e => new NotificationTypeLookup
            {
                Id = e,
                Name = e.ToString()
            })
        );
    }
}
