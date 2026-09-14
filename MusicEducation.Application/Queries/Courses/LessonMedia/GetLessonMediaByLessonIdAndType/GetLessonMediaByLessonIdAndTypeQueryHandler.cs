using MusicEducation.Application.DTOs.Courses;
using MusicEducation.Application.Exceptions;
using MusicEducation.Application.Mappings.Courses;
using MusicEducation.Domain.Interfaces.Courses;

namespace MusicEducation.Application.Queries.Courses.Lessons.Media;

public sealed class GetLessonMediaByLessonIdAndTypeQueryHandler
{
    private readonly ILessonMediaRepository _lessonMediaRepository;

    public GetLessonMediaByLessonIdAndTypeQueryHandler(ILessonMediaRepository lessonMediaRepository)
    {
        _lessonMediaRepository = lessonMediaRepository;
    }

    public async Task<IEnumerable<LessonMediaDto>> HandleAsync(GetLessonMediaByLessonIdAndTypeQuery query)
    {
        if (query.LessonId <= 0)
            throw new ValidationException("شناسه جلسه معتبر نیست");

        var media = await _lessonMediaRepository.GetByLessonIdAndTypeAsync(
                query.LessonId,
                query.MediaType);

        return media
            .OrderBy(x => x.Order)
            .Select(x => x.ToDto());
    }
}