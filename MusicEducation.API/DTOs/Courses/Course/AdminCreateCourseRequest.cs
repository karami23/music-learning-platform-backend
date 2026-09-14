namespace MusicEducation.API.DTOs.Courses.Course
{
    public sealed record AdminCreateCourseRequest(
        int TeacherId,
        int CourseCategoryId,
        string Title,
        string Slug,
        string Description,
        CoursePricingType PricingType,
        decimal? PriceAmount,
        string? CoverImageUrl);
}
