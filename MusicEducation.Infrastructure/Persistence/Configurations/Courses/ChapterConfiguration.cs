using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MusicEducation.Domain.Constants;
using MusicEducation.Domain.Entities.Courses;

namespace MusicEducation.Infrastructure.Persistence.Configurations.Courses;

public class ChapterConfiguration : IEntityTypeConfiguration<Chapter>
{
    public void Configure(EntityTypeBuilder<Chapter> builder)
    {
        builder.ToTable("Chapters");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.CourseId)
            .IsRequired();

        builder.Property(x => x.Title)
            .HasMaxLength(DomainConstants.MaxChapterTitleLength)
            .IsRequired();

        builder.Property(x => x.Description)
            .HasMaxLength(DomainConstants.MaxChapterDescriptionLength);

        builder.Property(x => x.Order)
            .IsRequired();

        builder.HasOne<Course>()
            .WithMany()
            .HasForeignKey(x => x.CourseId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasIndex(x => new
        {
            x.CourseId,
            x.Order
        })
        .IsUnique();
    }
}