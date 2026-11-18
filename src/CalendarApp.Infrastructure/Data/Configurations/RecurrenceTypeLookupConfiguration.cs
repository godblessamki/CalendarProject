namespace CalendarApp.Infrastructure.Data.Configurations;

using CalendarApp.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

public class RecurrenceTypeLookupConfiguration : IEntityTypeConfiguration<RecurrenceTypeLookup>
{
    public void Configure(EntityTypeBuilder<RecurrenceTypeLookup> builder)
    {
        builder.ToTable("RecurrenceTypes");

        builder.HasKey(r => r.Id);

        builder.Property(r => r.Id)
            .ValueGeneratedNever();

        builder.Property(r => r.Name)
            .IsRequired()
            .HasMaxLength(50);
    }
}
