namespace MusicEducation.Application.Commands.Learning.LessonProgress.UpdateLessonProgress;

public record UpdateLessonProgressCommand(
    int UserId,
    int LessonProgressId,
    int MediaId,
    TimeSpan Position,
    decimal ProgressPercentage);