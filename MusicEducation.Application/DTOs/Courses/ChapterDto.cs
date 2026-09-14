namespace MusicEducation.Application.DTOs.Courses;

public record ChapterDto(
    int Id,
    int CourseId,
    string Title,
    string? Description,
    int Order
);