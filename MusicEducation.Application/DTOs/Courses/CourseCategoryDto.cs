namespace MusicEducation.Application.DTOs.Courses;

public record CourseCategoryDto(
    int Id,
    string Name,
    string? Description,
    bool IsActive
);