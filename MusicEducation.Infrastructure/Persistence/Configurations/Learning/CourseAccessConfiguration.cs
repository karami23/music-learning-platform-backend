using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MusicEducation.Domain.Entities.Courses;
using MusicEducation.Domain.Entities.Identity;
using MusicEducation.Domain.Entities.Learning;

namespace MusicEducation.Infrastructure.Persistence.Configurations.Learning;

public class CourseAccessConfiguration : IEntityTypeConfiguration<CourseAccess>
{
    public void Configure(EntityTypeBuilder<CourseAccess> builder)
    {
        builder.ToTable("CourseAccesses");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.UserId)
            .IsRequired();

        builder.Property(x => x.CourseId)
            .IsRequired();

        builder.Property(x => x.Status)
            .HasConversion<int>()
            .IsRequired();

        builder.Property(x => x.GrantedAt)
            .IsRequired();

        builder.Property(x => x.RevokedAt);

        builder.HasOne<User>()
            .WithMany()
            .HasForeignKey(x => x.UserId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne<Course>()
            .WithMany()
            .HasForeignKey(x => x.CourseId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasIndex(x => new { x.UserId, x.CourseId })
            .IsUnique();
    }
}