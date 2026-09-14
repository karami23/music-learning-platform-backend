using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MusicEducation.Domain.Constants;
using MusicEducation.Domain.Entities.Courses;

namespace MusicEducation.Infrastructure.Persistence.Configurations.Courses;

public class CourseCategoryConfiguration : IEntityTypeConfiguration<CourseCategory>
{
    public void Configure(EntityTypeBuilder<CourseCategory> builder)
    {
        builder.ToTable("CourseCategories");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Name)
            .HasMaxLength(DomainConstants.MaxCourseCategoryNameLength)
            .IsRequired();

        builder.Property(x => x.Description)
            .HasMaxLength(DomainConstants.MaxCourseCategoryDescriptionLength);

        builder.Property(x => x.IsActive)
            .IsRequired();

        builder.HasIndex(x => x.Name)
            .IsUnique();
    }
}