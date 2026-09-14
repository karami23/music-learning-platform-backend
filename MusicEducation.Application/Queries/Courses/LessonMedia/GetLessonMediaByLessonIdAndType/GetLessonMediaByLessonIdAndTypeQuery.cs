using MusicEducation.Domain.Enums;

namespace MusicEducation.Application.Queries.Courses.Lessons.Media;

public record GetLessonMediaByLessonIdAndTypeQuery(
    int LessonId,
    LessonMediaType MediaType);