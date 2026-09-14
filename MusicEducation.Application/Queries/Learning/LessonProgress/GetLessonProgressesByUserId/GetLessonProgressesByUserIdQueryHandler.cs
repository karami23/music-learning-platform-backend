using MusicEducation.Application.DTOs.Learning;
using MusicEducation.Application.Exceptions;
using MusicEducation.Application.Mappings.Learning;
using MusicEducation.Domain.Interfaces.Learning;

namespace MusicEducation.Application.Queries.Learning.LessonProgress.GetLessonProgressesByUserId;

public sealed class GetLessonProgressesByUserIdQueryHandler
{
    private readonly ILessonProgressRepository _lessonProgressRepository;

    public GetLessonProgressesByUserIdQueryHandler(
        ILessonProgressRepository lessonProgressRepository)
    {
        _lessonProgressRepository = lessonProgressRepository;
    }

    public async Task<IEnumerable<LessonProgressDto>> Handle(
        GetLessonProgressesByUserIdQuery query)
    {
        if (query.UserId <= 0)
            throw new ValidationException(
                "شناسه کاربر معتبر نیست");

        var progresses =
            await _lessonProgressRepository
                .GetByUserIdAsync(query.UserId);

        return progresses
            .Select(x => x.ToDto())
            .ToList();
    }
}