using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MusicEducation.Domain.Constants;
using MusicEducation.Domain.Entities.Identity;

namespace MusicEducation.Infrastructure.Persistence.Configurations.Identity;

public class TeacherConfiguration : IEntityTypeConfiguration<Teacher>
{
    public void Configure(EntityTypeBuilder<Teacher> builder)
    {
        builder.ToTable("Teachers");

        builder.HasKey(x => x.Id);

        builder.HasOne<User>()
            .WithOne()
            .HasForeignKey<Teacher>(x => x.UserId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasIndex(x => x.UserId)
            .IsUnique();

        builder.Property(x => x.Bio)
            .HasMaxLength(DomainConstants.MaxTeacherBioLength);

        builder.Property(x => x.Status)
            .HasConversion<int>()
            .IsRequired();
    }
}