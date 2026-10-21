namespace CalendarApp.Infrastructure.Data.Configurations;

using CalendarApp.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

public class InvitationConfiguration : IEntityTypeConfiguration<Invitation>
{
    public void Configure(EntityTypeBuilder<Invitation> builder)
    {
        builder.HasKey(i => i.Id);

        builder.Property(i => i.Status)
            .IsRequired();

        builder.Property(i => i.SentAt)
            .IsRequired();

        builder.Property(i => i.Message)
            .HasMaxLength(500);

        builder.Property(i => i.InvitedByUserId)
            .IsRequired()
            .HasMaxLength(450);

        builder.HasOne(i => i.Event)
            .WithMany(e => e.Invitations)
            .HasForeignKey(i => i.EventId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(i => i.InvitedUser)
            .WithMany(u => u.Invitations)
            .HasForeignKey(i => i.InvitedUserId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasIndex(i => new { i.EventId, i.InvitedUserId })
            .IsUnique();

        builder.HasIndex(i => i.InvitedUserId);
        builder.HasIndex(i => new { i.InvitedUserId, i.Status });
        builder.HasIndex(i => i.InvitedByUserId);
    }
}
