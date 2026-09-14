namespace MusicEducation.API.DTOs.Courses.CourseCategory
{
    public sealed record CreateCourseCategoryRequest(
        string Name,
        string? Description);
}
