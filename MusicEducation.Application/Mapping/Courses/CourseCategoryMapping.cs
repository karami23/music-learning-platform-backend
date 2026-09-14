using MusicEducation.Application.DTOs.Courses;
using MusicEducation.Domain.Entities.Courses;

namespace MusicEducation.Application.Mappings.Courses;

public static class CourseCategoryMapping
{
    public static CourseCategoryDto ToDto(
        this CourseCategory category)
    {
        return new CourseCategoryDto(
            category.Id,
            category.Name,
            category.Description,
            category.IsActive
        );
    }
}