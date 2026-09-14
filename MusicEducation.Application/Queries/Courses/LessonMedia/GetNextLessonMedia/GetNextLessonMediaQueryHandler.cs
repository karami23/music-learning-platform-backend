using MusicEducation.Application.DTOs.Courses;
using MusicEducation.Application.Mappings.Courses;
using MusicEducation.Domain.Interfaces.Courses;

namespace MusicEducation.Application.Queries.Courses.Lessons.Media;

public sealed class GetNextLessonMediaQueryHandler
{
    private readonly ILessonMediaRepository _lessonMediaRepository;

    public GetNextLessonMediaQueryHandler(ILessonMediaRepository lessonMediaRepository)
    {
        _lessonMediaRepository = lessonMediaRepository;
    }

    public async Task<LessonMediaDto?> HandleAsync(GetNextLessonMediaQuery query)
    {
        if (query.MediaId <= 0)
            throw new Application.Exceptions.ValidationException("شناسه محتوا معتبر نیست");

        var media = await _lessonMediaRepository.GetNextMediaAsync(query.MediaId);

        return media?.ToDto();
    }
}