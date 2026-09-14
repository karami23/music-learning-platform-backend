namespace MusicEducation.API.DTOs.Learning.LessonProgress;

public sealed record CreateLessonProgressRequest(
    int LessonId);

public sealed record UpdateLessonProgressRequest(
    int MediaId,
    TimeSpan Position,
    decimal ProgressPercentage);