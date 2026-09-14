using MusicEducation.Application.DTOs.Learning;
using MusicEducation.Domain.Entities.Learning;

namespace MusicEducation.Application.Mappings.Learning;

public static class LessonProgressMapping
{
    public static LessonProgressDto ToDto(this LessonProgress progress)
    {
        return new LessonProgressDto(
            progress.Id,
            progress.UserId,
            progress.LessonId,
            progress.LastViewedMediaId,
            progress.LastPosition,
            progress.ProgressPercentage,
            progress.IsCompleted,
            progress.LastViewedAt
        );
    }
}