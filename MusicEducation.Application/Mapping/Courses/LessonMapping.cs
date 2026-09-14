using MusicEducation.Application.DTOs.Courses;
using MusicEducation.Domain.Entities.Courses;

namespace MusicEducation.Application.Mappings.Courses;

public static class LessonMapping
{
    public static LessonDto ToDto(this Lesson lesson)
    {
        return new LessonDto(
            lesson.Id,
            lesson.ChapterId,
            lesson.Title,
            lesson.Description,
            lesson.Order
        );
    }
}