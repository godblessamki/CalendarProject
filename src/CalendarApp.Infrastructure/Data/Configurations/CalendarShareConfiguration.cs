namespace CalendarApp.Infrastructure.Data.Configurations;

using CalendarApp.Domain.Entities;
using CalendarApp.Infrastructure.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

public class CalendarShareConfiguration : IEntityTypeConfiguration<CalendarShare>
{
    public void Configure(EntityTypeBuilder<CalendarShare> builder)
    {
        builder.HasKey(cs => cs.Id);

        builder.Property(cs => cs.AccessLevel)
            .IsRequired();

        builder.Property(cs => cs.SharedAt)
            .IsRequired();

        builder.Property(cs => cs.IsAccepted)
            .IsRequired();

        builder.HasOne(cs => cs.Calendar)
            .WithMany(c => c.Shares)
            .HasForeignKey(cs => cs.CalendarId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne<ApplicationUser>()
            .WithMany(u => u.SharedCalendars)
            .HasForeignKey(cs => cs.SharedWithUserId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne<ShareAccessLevelLookup>()
            .WithMany()
            .HasForeignKey(cs => cs.AccessLevel)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasIndex(cs => new { cs.CalendarId, cs.SharedWithUserId })
            .IsUnique();

        builder.HasIndex(cs => cs.SharedWithUserId);
    }
}
