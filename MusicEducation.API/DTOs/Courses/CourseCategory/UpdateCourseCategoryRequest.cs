namespace MusicEducation.API.DTOs.Courses.CourseCategory
{
    public sealed record UpdateCourseCategoryRequest(
        string Name,
        string? Description);
}
