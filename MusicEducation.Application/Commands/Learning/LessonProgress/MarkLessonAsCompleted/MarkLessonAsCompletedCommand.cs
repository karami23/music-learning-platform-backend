namespace MusicEducation.Application.Commands.Learning.LessonProgress.MarkLessonAsCompleted;

public record MarkLessonAsCompletedCommand(
    int UserId,
    int LessonProgressId);