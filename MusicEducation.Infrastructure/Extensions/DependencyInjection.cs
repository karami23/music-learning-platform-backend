using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MusicEducation.Application.Interfaces;
using MusicEducation.Application.Interfaces.Courses;
using MusicEducation.Application.Interfaces.Payment;
using MusicEducation.Domain.Interfaces.Articles;
using MusicEducation.Domain.Interfaces.Commerce;
using MusicEducation.Domain.Interfaces.Courses;
using MusicEducation.Domain.Interfaces.Identity;
using MusicEducation.Domain.Interfaces.Learning;
using MusicEducation.Domain.Interfaces.Notifications;
using MusicEducation.Domain.Interfaces.Reviews;
using MusicEducation.Infrastructure.Persistence;
using MusicEducation.Infrastructure.Persistence.DbContext;
using MusicEducation.Infrastructure.Repositories.Articles;
using MusicEducation.Infrastructure.Repositories.Commerce;
using MusicEducation.Infrastructure.Repositories.Courses;
using MusicEducation.Infrastructure.Repositories.Identity;
using MusicEducation.Infrastructure.Repositories.Learning;
using MusicEducation.Infrastructure.Repositories.Notifications;
using MusicEducation.Infrastructure.Repositories.Reviews;
using MusicEducation.Infrastructure.Services.Authentication;
using MusicEducation.Infrastructure.Services.Payments;
using MusicEducation.Infrastructure.Services.Sms;
using MusicEducation.Infrastructure.Services.Storage;
using MusicEducation.Infrastructure.Settings;

namespace MusicEducation.Infrastructure.DependencyInjection;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddDbContext<MusicEducationDbContext>(options =>
            options.UseSqlServer(
                configuration.GetConnectionString(
                    "DefaultConnection")));

        services.Configure<JwtSettings>(
            configuration.GetSection(
                JwtSettings.SectionName));

        services.Configure<SmsSettings>(
            configuration.GetSection(
                SmsSettings.SectionName));

        services.Configure<PaymentSettings>(
            configuration.GetSection(
                PaymentSettings.SectionName));

        services.Configure<StorageSettings>(
            configuration.GetSection(
                StorageSettings.SectionName));

        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<ITeacherRepository, TeacherRepository>();
        services.AddScoped<IRefreshTokenRepository, RefreshTokenRepository>();
        services.AddScoped<IVerificationCodeRepository, VerificationCodeRepository>();

        services.AddScoped<ICourseRepository, CourseRepository>();
        services.AddScoped<ICourseCategoryRepository, CourseCategoryRepository>();
        services.AddScoped<IChapterRepository, ChapterRepository>();
        services.AddScoped<ILessonRepository, LessonRepository>();
        services.AddScoped<ILessonMediaRepository, LessonMediaRepository>();
        services.AddScoped<ICourseReadRepository, CourseReadRepository>();

        services.AddScoped<ICartRepository, CartRepository>();
        services.AddScoped<ICartItemRepository, CartItemRepository>();
        services.AddScoped<IDiscountCodeRepository, DiscountCodeRepository>();
        services.AddScoped<IOrderRepository, OrderRepository>();
        services.AddScoped<IOrderItemRepository, OrderItemRepository>();
        services.AddScoped<IPaymentRepository, PaymentRepository>();

        services.AddScoped<ICourseAccessRepository, CourseAccessRepository>();
        services.AddScoped<ILessonProgressRepository, LessonProgressRepository>();

        services.AddScoped<IArticleCategoryRepository, ArticleCategoryRepository>();
        services.AddScoped<IArticleRepository, ArticleRepository>();

        services.AddScoped<IReviewRepository, ReviewRepository>();

        services.AddScoped<INotificationRepository, NotificationRepository>();

        services.AddScoped<IUnitOfWork, UnitOfWork>();

        services.AddScoped<IPasswordHasher, PasswordHasher>();
        services.AddScoped<ITokenService, TokenService>();
        services.AddScoped<ISmsService, SmsService>();
        services.AddScoped<IPaymentService, PaymentService>();
        services.AddScoped<IFileStorageService, FileStorageService>();

        return services;
    }
}