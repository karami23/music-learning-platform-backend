namespace MusicEducation.Application.Queries.Learning.LessonProgress.GetLessonProgressByUserAndLesson;

public record GetLessonProgressByUserAndLessonQuery(
    int UserId,
    int LessonId
);