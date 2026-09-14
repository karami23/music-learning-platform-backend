using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MusicEducation.Domain.Constants;
using MusicEducation.Domain.Entities.Identity;
using MusicEducation.Domain.Entities.Notifications;

namespace MusicEducation.Infrastructure.Persistence.Configurations.Notifications;

public class NotificationConfiguration : IEntityTypeConfiguration<Notification>
{
    public void Configure(EntityTypeBuilder<Notification> builder)
    {
        builder.ToTable("Notifications");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.UserId)
            .IsRequired();

        builder.Property(x => x.Type)
            .HasConversion<int>()
            .IsRequired();

        builder.Property(x => x.Title)
            .HasMaxLength(DomainConstants.MaxNotificationTitleLength)
            .IsRequired();

        builder.Property(x => x.Message)
            .HasMaxLength(DomainConstants.MaxNotificationMessageLength)
            .IsRequired();

        builder.Property(x => x.ActionUrl)
            .HasMaxLength(DomainConstants.MaxNotificationActionUrlLength);

        builder.Property(x => x.IsRead)
            .IsRequired();

        builder.Property(x => x.ReadAt);

        builder.HasOne<User>()
            .WithMany()
            .HasForeignKey(x => x.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasIndex(x => new { x.UserId, x.IsRead });
    }
}