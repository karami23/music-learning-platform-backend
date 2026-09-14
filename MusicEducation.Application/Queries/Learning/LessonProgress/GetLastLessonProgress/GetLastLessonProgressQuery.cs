namespace MusicEducation.Application.Queries.Learning.LessonProgress.GetLastLessonProgress;

public record GetLastLessonProgressQuery(
    int UserId,
    int CourseId
);