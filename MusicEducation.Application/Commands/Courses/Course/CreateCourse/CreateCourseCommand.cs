namespace MusicEducation.Application.Commands.Courses.Course.CreateCourse
{
    public sealed record CreateCourseCommand(
        int TeacherId,
        int CourseCategoryId,
        string Title,
        string Slug,
        string Description,
        CoursePricingType PricingType,
        decimal? PriceAmount,
        string? CoverImageUrl
    );
}
