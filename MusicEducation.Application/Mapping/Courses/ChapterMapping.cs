using MusicEducation.Application.DTOs.Courses;
using MusicEducation.Domain.Entities.Courses;

namespace MusicEducation.Application.Mappings.Courses;

public static class ChapterMapping
{
    public static ChapterDto ToDto(this Chapter chapter)
    {
        return new ChapterDto(
            chapter.Id,
            chapter.CourseId,
            chapter.Title,
            chapter.Description,
            chapter.Order
        );
    }
}