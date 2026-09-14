using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MusicEducation.Application.Interfaces;
using MusicEducation.Infrastructure.Persistence.DbContext;

namespace MusicEducation.Infrastructure.Persistence.Seed;

public static class DatabaseSeeder
{
    public static async Task SeedAsync(
        IServiceProvider serviceProvider,
        IConfiguration configuration,
        CancellationToken cancellationToken = default)
    {
        using var scope = serviceProvider.CreateScope();

        var context = scope.ServiceProvider
            .GetRequiredService<MusicEducationDbContext>();

        var passwordHasher = scope.ServiceProvider
            .GetRequiredService<IPasswordHasher>();

        await AdminSeed.SeedAsync(
            context,
            configuration,
            passwordHasher,
            cancellationToken);
    }
}