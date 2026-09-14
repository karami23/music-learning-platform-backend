namespace MusicEducation.Application.DTOs.Courses;

public record LessonDto(
    int Id,
    int ChapterId,
    string Title,
    string? Description,
    int Order
);