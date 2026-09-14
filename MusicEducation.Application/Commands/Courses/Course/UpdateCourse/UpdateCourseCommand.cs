namespace MusicEducation.Application.Commands.Courses.Course.UpdateCourse
{
    public sealed record UpdateCourseCommand(
        int CourseId,
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
