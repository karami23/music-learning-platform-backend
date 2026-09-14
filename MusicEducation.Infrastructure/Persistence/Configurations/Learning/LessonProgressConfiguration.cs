using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MusicEducation.Domain.Entities.Courses;
using MusicEducation.Domain.Entities.Identity;
using MusicEducation.Domain.Entities.Learning;

namespace MusicEducation.Infrastructure.Persistence.Configurations.Learning;

public class LessonProgressConfiguration : IEntityTypeConfiguration<LessonProgress>
{
    public void Configure(EntityTypeBuilder<LessonProgress> builder)
    {
        builder.ToTable("LessonProgresses");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.UserId)
            .IsRequired();

        builder.Property(x => x.LessonId)
            .IsRequired();

        builder.Property(x => x.LastViewedMediaId);

        builder.Property(x => x.LastPosition)
            .IsRequired();

        builder.Property(x => x.ProgressPercentage)
            .HasPrecision(5, 2)
            .IsRequired();

        builder.Property(x => x.IsCompleted)
            .IsRequired();

        builder.Property(x => x.LastViewedAt)
            .IsRequired();

        builder.HasOne<User>()
            .WithMany()
            .HasForeignKey(x => x.UserId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne<Lesson>()
            .WithMany()
            .HasForeignKey(x => x.LessonId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne<LessonMedia>()
            .WithMany()
            .HasForeignKey(x => x.LastViewedMediaId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasIndex(x => new { x.UserId, x.LessonId })
            .IsUnique();
    }
}