namespace CalendarApp.Infrastructure.Data.Configurations;

using CalendarApp.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

public class NotificationTypeLookupConfiguration : IEntityTypeConfiguration<NotificationTypeLookup>
{
    public void Configure(EntityTypeBuilder<NotificationTypeLookup> builder)
    {
        builder.ToTable("NotificationTypes");

        builder.HasKey(n => n.Id);

        builder.Property(n => n.Id)
            .ValueGeneratedNever();

        builder.Property(n => n.Name)
            .IsRequired()
            .HasMaxLength(50);
    }
}
