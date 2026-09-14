namespace MusicEducation.Application.Commands.Learning.LessonProgress.ResetLessonProgress;

public record ResetLessonProgressCommand(
    int UserId,
    int LessonProgressId);