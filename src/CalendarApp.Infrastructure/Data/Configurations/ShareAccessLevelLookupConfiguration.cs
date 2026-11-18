namespace CalendarApp.Infrastructure.Data.Configurations;

using CalendarApp.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

public class ShareAccessLevelLookupConfiguration : IEntityTypeConfiguration<ShareAccessLevelLookup>
{
    public void Configure(EntityTypeBuilder<ShareAccessLevelLookup> builder)
    {
        builder.ToTable("ShareAccessLevels");

        builder.HasKey(s => s.Id);

        builder.Property(s => s.Id)
            .ValueGeneratedNever();

        builder.Property(s => s.Name)
            .IsRequired()
            .HasMaxLength(50);
    }
}
