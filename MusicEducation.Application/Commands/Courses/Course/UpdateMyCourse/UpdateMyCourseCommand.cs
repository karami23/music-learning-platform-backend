namespace MusicEducation.Application.Commands.Courses.Course.UpdateMyCourse;

public sealed record UpdateMyCourseCommand(
    int UserId,
    int CourseId,
    int CourseCategoryId,
    string Title,
    string Slug,
    string Description,
    CoursePricingType PricingType,
    decimal? PriceAmount,
    string? CoverImageUrl);