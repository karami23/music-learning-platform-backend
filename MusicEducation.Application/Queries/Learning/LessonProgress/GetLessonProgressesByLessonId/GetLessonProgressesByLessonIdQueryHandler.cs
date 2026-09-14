using MusicEducation.Application.DTOs.Learning;
using MusicEducation.Application.Exceptions;
using MusicEducation.Application.Mappings.Learning;
using MusicEducation.Domain.Interfaces.Learning;

namespace MusicEducation.Application.Queries.Learning.LessonProgress.GetLessonProgressesByLessonId;

public sealed class GetLessonProgressesByLessonIdQueryHandler
{
    private readonly ILessonProgressRepository _lessonProgressRepository;

    public GetLessonProgressesByLessonIdQueryHandler(
        ILessonProgressRepository lessonProgressRepository)
    {
        _lessonProgressRepository = lessonProgressRepository;
    }

    public async Task<IEnumerable<LessonProgressDto>> Handle(
        GetLessonProgressesByLessonIdQuery query)
    {
        if (query.LessonId <= 0)
            throw new ValidationException(
                "شناسه جلسه معتبر نیست");

        var progresses =
            await _lessonProgressRepository
                .GetByLessonIdAsync(query.LessonId);

        return progresses
            .Select(x => x.ToDto())
            .ToList();
    }
}