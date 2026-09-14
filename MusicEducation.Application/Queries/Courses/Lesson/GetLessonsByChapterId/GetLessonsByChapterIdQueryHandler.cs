using MusicEducation.Application.DTOs.Courses;
using MusicEducation.Application.Exceptions;
using MusicEducation.Application.Mappings.Courses;
using MusicEducation.Domain.Interfaces.Courses;

namespace MusicEducation.Application.Queries.Courses.Lessons;

public sealed class GetLessonsByChapterIdQueryHandler
{
    private readonly ILessonRepository _lessonRepository;

    public GetLessonsByChapterIdQueryHandler(ILessonRepository lessonRepository)
    {
        _lessonRepository = lessonRepository;
    }

    public async Task<IEnumerable<LessonDto>> Handle(
        GetLessonsByChapterIdQuery query,
        CancellationToken cancellationToken = default)
    {
        if (query.ChapterId <= 0)
            throw new ValidationException("شناسه فصل معتبر نیست");

        var lessons = await _lessonRepository.GetByChapterIdAsync(query.ChapterId);

        return lessons
            .OrderBy(x => x.Order)
            .Select(x => x.ToDto());
    }
}