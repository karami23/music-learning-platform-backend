using MusicEducation.Application.DTOs.Learning;
using MusicEducation.Application.Exceptions;
using MusicEducation.Application.Mappings.Learning;
using MusicEducation.Domain.Interfaces.Learning;

namespace MusicEducation.Application.Queries.Learning.LessonProgress.GetLessonProgressById;

public sealed class GetLessonProgressByIdQueryHandler
{
    private readonly ILessonProgressRepository _lessonProgressRepository;

    public GetLessonProgressByIdQueryHandler(
        ILessonProgressRepository lessonProgressRepository)
    {
        _lessonProgressRepository = lessonProgressRepository;
    }

    public async Task<LessonProgressDto?> Handle(
        GetLessonProgressByIdQuery query)
    {
        if (query.LessonProgressId <= 0)
            throw new ValidationException(
                "شناسه پیشرفت معتبر نیست");

        var progress =
            await _lessonProgressRepository
                .GetByIdAsync(query.LessonProgressId);

        return progress?.ToDto();
    }
}