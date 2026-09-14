using MusicEducation.Application.DTOs.Courses;
using MusicEducation.Application.Mappings.Courses;
using MusicEducation.Domain.Interfaces.Courses;

namespace MusicEducation.Application.Queries.Courses.Lessons;

public sealed class GetAllLessonsQueryHandler
{
    private readonly ILessonRepository _lessonRepository;

    public GetAllLessonsQueryHandler(ILessonRepository lessonRepository)
    {
        _lessonRepository = lessonRepository;
    }

    public async Task<IEnumerable<LessonDto>> Handle(
        GetAllLessonsQuery query,
        CancellationToken cancellationToken = default)
    {
        var lessons = await _lessonRepository.GetAllAsync();

        return lessons
            .OrderBy(x => x.ChapterId)
            .ThenBy(x => x.Order)
            .Select(x => x.ToDto());
    }
}