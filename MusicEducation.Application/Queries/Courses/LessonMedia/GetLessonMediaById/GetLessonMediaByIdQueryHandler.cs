using MusicEducation.Application.DTOs.Courses;
using MusicEducation.Application.Exceptions;
using MusicEducation.Application.Mappings.Courses;
using MusicEducation.Domain.Interfaces.Courses;

namespace MusicEducation.Application.Queries.Courses.Lessons.Media;

public sealed class GetLessonMediaByIdQueryHandler
{
    private readonly ILessonMediaRepository _lessonMediaRepository;

    public GetLessonMediaByIdQueryHandler(ILessonMediaRepository lessonMediaRepository)
    {
        _lessonMediaRepository = lessonMediaRepository;
    }

    public async Task<LessonMediaDto> HandleAsync(GetLessonMediaByIdQuery query)
    {
        if (query.MediaId <= 0)
            throw new ValidationException("شناسه محتوا معتبر نیست");

        var media = await _lessonMediaRepository.GetByIdAsync(query.MediaId);

        if (media is null)
            throw new NotFoundException("محتوای آموزشی موردنظر پیدا نشد");

        return media.ToDto();
    }
}