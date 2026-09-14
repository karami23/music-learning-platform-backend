using MusicEducation.Application.DTOs.Learning;
using MusicEducation.Application.Exceptions;
using MusicEducation.Application.Mappings.Learning;
using MusicEducation.Domain.Interfaces.Learning;

namespace MusicEducation.Application.Queries.Learning.LessonProgress.GetLessonProgressByUserAndLesson;

public sealed class GetLessonProgressByUserAndLessonQueryHandler
{
    private readonly ILessonProgressRepository _lessonProgressRepository;

    public GetLessonProgressByUserAndLessonQueryHandler(
        ILessonProgressRepository lessonProgressRepository)
    {
        _lessonProgressRepository = lessonProgressRepository;
    }

    public async Task<LessonProgressDto?> Handle(
        GetLessonProgressByUserAndLessonQuery query)
    {
        if (query.UserId <= 0)
            throw new ValidationException(
                "شناسه کاربر معتبر نیست");

        if (query.LessonId <= 0)
            throw new ValidationException(
                "شناسه جلسه معتبر نیست");

        var progress =
            await _lessonProgressRepository
                .GetByUserIdAndLessonIdAsync(
                    query.UserId,
                    query.LessonId);

        return progress?.ToDto();
    }
}