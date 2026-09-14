using MusicEducation.Application.DTOs.Courses;
using MusicEducation.Domain.Entities.Courses;

namespace MusicEducation.Application.Mappings.Courses;

public static class LessonMediaMapping
{
    public static LessonMediaDto ToDto(this LessonMedia media)
    {
        return new LessonMediaDto(
            media.Id,
            media.LessonId,
            media.MediaType,
            media.Title,
            media.FileName,
            media.StorageKey,
            media.Order,
            media.Duration
        );
    }
}