using MusicEducation.Application.DTOs.Courses;
using MusicEducation.Application.Exceptions;
using MusicEducation.Application.Mappings.Courses;
using MusicEducation.Domain.Interfaces.Courses;

namespace MusicEducation.Application.Queries.Courses.Lessons;

public sealed class GetLessonByIdQueryHandler
{
    private readonly ILessonRepository _lessonRepository;

    public GetLessonByIdQueryHandler(ILessonRepository lessonRepository)
    {
        _lessonRepository = lessonRepository;
    }

    public async Task<LessonDto> Handle(
        GetLessonByIdQuery query,
        CancellationToken cancellationToken = default)
    {
        if (query.LessonId <= 0)
            throw new ValidationException("شناسه جلسه معتبر نیست");

        var lesson = await _lessonRepository.GetByIdAsync(query.LessonId);

        if (lesson is null)
            throw new NotFoundException("جلسه مورد نظر پیدا نشد");

        return lesson.ToDto();
    }
}