namespace CalendarApp.Infrastructure.Data.Configurations;

using CalendarApp.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

public class EventConfiguration : IEntityTypeConfiguration<Event>
{
    public void Configure(EntityTypeBuilder<Event> builder)
    {
        builder.HasKey(e => e.Id);

        builder.Property(e => e.Title)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(e => e.Description)
            .HasMaxLength(2000);

        builder.Property(e => e.Location)
            .HasMaxLength(255);

        builder.Property(e => e.StartDate)
            .IsRequired();

        builder.Property(e => e.EndDate)
            .IsRequired();

        builder.Property(e => e.IsAllDay)
            .IsRequired();

        builder.Property(e => e.Recurrence)
            .IsRequired();

        builder.Property(e => e.CreatedAt)
            .IsRequired();

        builder.Property(e => e.UpdatedAt)
            .IsRequired();

        builder.HasOne(e => e.Calendar)
            .WithMany(c => c.Events)
            .HasForeignKey(e => e.CalendarId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(e => e.Category)
            .WithMany(c => c.Events)
            .HasForeignKey(e => e.CategoryId)
            .OnDelete(DeleteBehavior.SetNull);

        builder.HasMany(e => e.Notifications)
            .WithOne(n => n.Event)
            .HasForeignKey(n => n.EventId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(e => e.Invitations)
            .WithOne(i => i.Event)
            .HasForeignKey(i => i.EventId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasIndex(e => e.CalendarId);
        builder.HasIndex(e => e.CategoryId);
        builder.HasIndex(e => e.StartDate);
        builder.HasIndex(e => e.EndDate);
    }
}
