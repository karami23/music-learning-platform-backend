using MusicEducation.Application.DTOs.Courses;
using MusicEducation.Application.Mappings.Courses;
using MusicEducation.Domain.Interfaces.Courses;

namespace MusicEducation.Application.Queries.Courses.Lessons.Media;

public sealed class GetAllLessonMediaQueryHandler
{
    private readonly ILessonMediaRepository _lessonMediaRepository;

    public GetAllLessonMediaQueryHandler(ILessonMediaRepository lessonMediaRepository)
    {
        _lessonMediaRepository = lessonMediaRepository;
    }

    public async Task<IEnumerable<LessonMediaDto>> HandleAsync(GetAllLessonMediaQuery query)
    {
        var media = await _lessonMediaRepository.GetAllAsync();

        return media
            .OrderBy(x => x.LessonId)
            .ThenBy(x => x.Order)
            .Select(x => x.ToDto());
    }
}