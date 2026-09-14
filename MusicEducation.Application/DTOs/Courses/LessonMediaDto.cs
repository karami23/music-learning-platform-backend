namespace MusicEducation.Application.DTOs.Courses;

public record LessonMediaDto(
    int Id,
    int LessonId,
    LessonMediaType MediaType,
    string Title,
    string FileName,
    string StorageKey,
    int Order,
    TimeSpan? Duration
);