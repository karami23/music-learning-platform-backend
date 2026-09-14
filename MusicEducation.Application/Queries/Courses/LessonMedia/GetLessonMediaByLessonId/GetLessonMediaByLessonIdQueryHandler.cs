using MusicEducation.Application.DTOs.Courses;
using MusicEducation.Application.Exceptions;
using MusicEducation.Application.Mappings.Courses;
using MusicEducation.Domain.Interfaces.Courses;

namespace MusicEducation.Application.Queries.Courses.Lessons.Media;

public sealed class GetLessonMediaByLessonIdQueryHandler
{
    private readonly ILessonMediaRepository _lessonMediaRepository;

    public GetLessonMediaByLessonIdQueryHandler(ILessonMediaRepository lessonMediaRepository)
    {
        _lessonMediaRepository = lessonMediaRepository;
    }

    public async Task<IEnumerable<LessonMediaDto>> HandleAsync(GetLessonMediaByLessonIdQuery query)
    {
        if (query.LessonId <= 0)
            throw new ValidationException("شناسه جلسه معتبر نیست");

        var media = await _lessonMediaRepository.GetByLessonIdAsync(query.LessonId);

        return media
            .OrderBy(x => x.Order)
            .Select(x => x.ToDto());
    }
}