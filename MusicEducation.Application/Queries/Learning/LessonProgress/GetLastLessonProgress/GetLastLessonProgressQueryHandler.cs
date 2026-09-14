using MusicEducation.Application.DTOs.Learning;
using MusicEducation.Application.Exceptions;
using MusicEducation.Application.Mappings.Learning;
using MusicEducation.Domain.Interfaces.Learning;

namespace MusicEducation.Application.Queries.Learning.LessonProgress.GetLastLessonProgress;

public sealed class GetLastLessonProgressQueryHandler
{
    private readonly ILessonProgressRepository _lessonProgressRepository;

    public GetLastLessonProgressQueryHandler(
        ILessonProgressRepository lessonProgressRepository)
    {
        _lessonProgressRepository = lessonProgressRepository;
    }

    public async Task<LessonProgressDto?> Handle(
        GetLastLessonProgressQuery query)
    {
        if (query.UserId <= 0)
            throw new ValidationException(
                "شناسه کاربر معتبر نیست");

        if (query.CourseId <= 0)
            throw new ValidationException(
                "شناسه دوره معتبر نیست");

        var progress =
            await _lessonProgressRepository
                .GetLastProgressAsync(
                    query.UserId,
                    query.CourseId);

        return progress?.ToDto();
    }
}
