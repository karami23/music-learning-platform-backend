using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MusicEducation.Domain.Entities.Courses;
using MusicEducation.Domain.Entities.Identity;

namespace MusicEducation.Infrastructure.Persistence.Configurations.Courses;

public class CourseConfiguration : IEntityTypeConfiguration<Course>
{
    public void Configure(EntityTypeBuilder<Course> builder)
    {
        builder.ToTable("Courses");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.TeacherId)
            .IsRequired();

        builder.Property(x => x.CourseCategoryId)
            .IsRequired();

        builder.Property(x => x.Title)
            .HasMaxLength(200)
            .IsRequired();

        builder.Property(x => x.Slug)
            .HasMaxLength(250)
            .IsRequired();

        builder.HasIndex(x => x.Slug)
            .IsUnique();

        builder.Property(x => x.Description)
            .HasMaxLength(5000)
            .IsRequired();

        // Price - Money Value Object
        builder.OwnsOne(x => x.Price, money =>
        {
            money.Property(x => x.Amount)
                .HasPrecision(18, 2)
                .IsRequired();

            money.Property(x => x.Currency)
                .HasMaxLength(10)
                .IsRequired();
        });

        builder.Property(x => x.PricingType)
            .HasConversion<int>()
            .IsRequired();

        builder.Property(x => x.Status)
            .HasConversion<int>()
            .IsRequired();

        builder.Property(x => x.CoverImageUrl)
            .HasMaxLength(500);

        builder.HasOne<Teacher>()
            .WithMany()
            .HasForeignKey(x => x.TeacherId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne<CourseCategory>()
            .WithMany()
            .HasForeignKey(x => x.CourseCategoryId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}