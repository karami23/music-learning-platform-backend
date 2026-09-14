using MusicEducation.Application.DTOs.Courses;
using MusicEducation.Application.Exceptions;
using MusicEducation.Application.Mappings.Courses;
using MusicEducation.Domain.Interfaces.Courses;

namespace MusicEducation.Application.Queries.Courses.Lessons.Media;

public sealed class GetFirstLessonMediaQueryHandler
{
    private readonly ILessonMediaRepository _lessonMediaRepository;

    public GetFirstLessonMediaQueryHandler(ILessonMediaRepository lessonMediaRepository)
    {
        _lessonMediaRepository = lessonMediaRepository;
    }

    public async Task<LessonMediaDto> HandleAsync(GetFirstLessonMediaQuery query)
    {
        if (query.LessonId <= 0)
            throw new ValidationException("شناسه جلسه معتبر نیست");

        var media = await _lessonMediaRepository.GetFirstMediaAsync(query.LessonId);

        if (media is null)
            throw new NotFoundException("هیچ محتوایی برای این جلسه پیدا نشد");

        return media.ToDto();
    }
}