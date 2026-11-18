namespace CalendarApp.Infrastructure.Data.Configurations;

using CalendarApp.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

public class InvitationStatusLookupConfiguration : IEntityTypeConfiguration<InvitationStatusLookup>
{
    public void Configure(EntityTypeBuilder<InvitationStatusLookup> builder)
    {
        builder.ToTable("InvitationStatuses");

        builder.HasKey(i => i.Id);

        builder.Property(i => i.Id)
            .ValueGeneratedNever();

        builder.Property(i => i.Name)
            .IsRequired()
            .HasMaxLength(50);
    }
}
