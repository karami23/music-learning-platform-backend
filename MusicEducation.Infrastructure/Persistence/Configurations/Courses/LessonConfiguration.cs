using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MusicEducation.Domain.Constants;
using MusicEducation.Domain.Entities.Courses;

namespace MusicEducation.Infrastructure.Persistence.Configurations.Courses;

public class LessonConfiguration : IEntityTypeConfiguration<Lesson>
{
    public void Configure(EntityTypeBuilder<Lesson> builder)
    {
        builder.ToTable("Lessons");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.ChapterId)
            .IsRequired();

        builder.Property(x => x.Title)
            .HasMaxLength(DomainConstants.MaxLessonTitleLength)
            .IsRequired();

        builder.Property(x => x.Description)
            .HasMaxLength(DomainConstants.MaxLessonDescriptionLength);

        builder.Property(x => x.Order)
            .IsRequired();

        builder.HasOne<Chapter>()
            .WithMany()
            .HasForeignKey(x => x.ChapterId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasIndex(x => new
        {
            x.ChapterId,
            x.Order
        })
        .IsUnique();
    }
}