namespace MusicEducation.Application.DTOs.Learning;

public record LessonProgressDto(
    int Id,
    int UserId,
    int LessonId,
    int? LastViewedMediaId,
    TimeSpan LastPosition,
    decimal ProgressPercentage,
    bool IsCompleted,
    DateTime LastViewedAt
);