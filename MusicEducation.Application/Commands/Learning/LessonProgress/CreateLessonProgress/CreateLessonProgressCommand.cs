namespace MusicEducation.Application.Commands.Learning.LessonProgress.CreateLessonProgress;

public record CreateLessonProgressCommand(
    int UserId,
    int LessonId
);