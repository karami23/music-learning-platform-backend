using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MusicEducation.Domain.Entities.Identity;
using MusicEducation.Domain.ValueObjects;

namespace MusicEducation.Infrastructure.Persistence.Configurations.Identity;

public class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.ToTable("Users");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Email)
            .HasConversion(
                email => email.Value,
                value => Email.Create(value))
            .HasMaxLength(256)
            .IsRequired();

        builder.HasIndex(x => x.Email)
            .IsUnique();

        builder.Property(x => x.PasswordHash)
            .IsRequired();

        builder.Property(x => x.FirstName)
            .HasMaxLength(100)
            .IsRequired();

        builder.Property(x => x.LastName)
            .HasMaxLength(100)
            .IsRequired();

        builder.OwnsOne(
            x => x.PhoneNumber,
            phoneBuilder =>
            {
                phoneBuilder.Property(x => x.CountryCode)
                    .HasColumnName("PhoneCountryCode")
                    .HasMaxLength(4)
                    .IsRequired();

                phoneBuilder.Property(x => x.NationalNumber)
                    .HasColumnName("PhoneNationalNumber")
                    .HasMaxLength(15)
                    .IsRequired();

                phoneBuilder.HasIndex(x => new
                {
                    x.CountryCode,
                    x.NationalNumber
                })
                .IsUnique();
            });

        builder.Property(x => x.ProfileImageUrl)
            .HasMaxLength(500);

        builder.Property(x => x.Role)
            .HasConversion<int>()
            .IsRequired();

        builder.Property(x => x.Status)
            .HasConversion<int>()
            .IsRequired();

        builder.Property(x => x.IsPhoneVerified)
            .IsRequired();
    }
}