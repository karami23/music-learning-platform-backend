using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using MusicEducation.Application.Interfaces;
using MusicEducation.Domain.Entities.Identity;
using MusicEducation.Domain.Enums;
using MusicEducation.Domain.ValueObjects;
using MusicEducation.Infrastructure.Persistence.DbContext;

namespace MusicEducation.Infrastructure.Persistence.Seed;

public static class AdminSeed
{
    public static async Task SeedAsync(
        MusicEducationDbContext context,
        IConfiguration configuration,
        IPasswordHasher passwordHasher,
        CancellationToken cancellationToken = default)
    {
        var adminEmail = configuration["Admin:Email"];
        var adminPassword = configuration["Admin:Password"];
        var adminFirstName = configuration["Admin:FirstName"];
        var adminLastName = configuration["Admin:LastName"];

        if (string.IsNullOrWhiteSpace(adminEmail))
            throw new InvalidOperationException(
                "Admin:Email در تنظیمات برنامه مشخص نشده است.");

        if (string.IsNullOrWhiteSpace(adminPassword))
            throw new InvalidOperationException(
                "Admin:Password در تنظیمات برنامه مشخص نشده است.");

        if (string.IsNullOrWhiteSpace(adminFirstName))
            throw new InvalidOperationException(
                "Admin:FirstName در تنظیمات برنامه مشخص نشده است.");

        if (string.IsNullOrWhiteSpace(adminLastName))
            throw new InvalidOperationException(
                "Admin:LastName در تنظیمات برنامه مشخص نشده است.");

        var email = Email.Create(adminEmail);

        var adminExists = await context.Set<User>()
            .AnyAsync(
                x => x.Email == email,
                cancellationToken);

        if (adminExists)
            return;

        var passwordHash = passwordHasher.HashPassword(adminPassword);

        var admin = User.Create(
            email: email,
            passwordHash: passwordHash,
            firstName: adminFirstName,
            lastName: adminLastName,
            role: UserRole.Admin);

        await context.Set<User>().AddAsync(admin, cancellationToken);

        await context.SaveChangesAsync(cancellationToken);
    }
}