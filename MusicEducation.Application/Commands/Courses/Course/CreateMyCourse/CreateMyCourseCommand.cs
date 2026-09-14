namespace MusicEducation.Application.Commands.Courses.Course.CreateMyCourse;

public sealed record CreateMyCourseCommand(
    int UserId,
    int CourseCategoryId,
    string Title,
    string Slug,
    string Description,
    CoursePricingType PricingType,
    decimal? PriceAmount,
    string? CoverImageUrl);
