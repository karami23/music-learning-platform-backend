using MusicEducation.Application.DTOs.Courses;
using MusicEducation.Application.Exceptions;
using MusicEducation.Application.Interfaces.Courses;
using MusicEducation.Application.Mappings.Courses;

namespace MusicEducation.Application.Queries.Courses;

public sealed class GetCourseByIdHandler
{
    private readonly ICourseReadRepository _courseReadRepository;

    public GetCourseByIdHandler(ICourseReadRepository courseReadRepository)
    {
        _courseReadRepository = courseReadRepository;
    }

    public async Task<CourseDetailsDto> HandleAsync(GetCourseByIdQuery query)
    {
        if (query.CourseId <= 0)
            throw new ValidationException("شناسه دوره معتبر نیست");

        var course = await _courseReadRepository.GetByIdAsync(query.CourseId);

        if (course is null)
            throw new NotFoundException("دوره پیدا نشد");

        return course.ToDto();
    }
}