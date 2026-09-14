using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MusicEducation.Domain.Constants;
using MusicEducation.Domain.Entities.Courses;

namespace MusicEducation.Infrastructure.Persistence.Configurations.Courses;

public class LessonMediaConfiguration : IEntityTypeConfiguration<LessonMedia>
{
    public void Configure(EntityTypeBuilder<LessonMedia> builder)
    {
        builder.ToTable("LessonMedia");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.LessonId)
            .IsRequired();

        builder.Property(x => x.MediaType)
            .HasConversion<int>()
            .IsRequired();

        builder.Property(x => x.Title)
            .HasMaxLength(DomainConstants.MaxLessonMediaTitleLength)
            .IsRequired();

        builder.Property(x => x.FileName)
            .HasMaxLength(DomainConstants.MaxLessonMediaFileNameLength)
            .IsRequired();

        builder.Property(x => x.StorageKey)
            .HasMaxLength(DomainConstants.MaxLessonMediaStorageKeyLength)
            .IsRequired();

        builder.Property(x => x.Order)
            .IsRequired();

        builder.Property(x => x.Duration);

        builder.HasOne<Lesson>()
            .WithMany()
            .HasForeignKey(x => x.LessonId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasIndex(x => new
        {
            x.LessonId,
            x.Order
        })
        .IsUnique();
    }
}