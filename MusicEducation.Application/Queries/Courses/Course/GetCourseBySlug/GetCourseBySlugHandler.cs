using MusicEducation.Application.DTOs.Courses;
using MusicEducation.Application.Exceptions;
using MusicEducation.Application.Interfaces.Courses;
using MusicEducation.Application.Mappings.Courses;

namespace MusicEducation.Application.Queries.Courses;

public sealed class GetCourseBySlugHandler
{
    private readonly ICourseReadRepository _courseReadRepository;

    public GetCourseBySlugHandler(ICourseReadRepository courseReadRepository)
    {
        _courseReadRepository = courseReadRepository;
    }

    public async Task<CourseDetailsDto> HandleAsync(GetCourseBySlugQuery query)
    {
        if (string.IsNullOrWhiteSpace(query.Slug))
            throw new ValidationException("Slug دوره الزامی است");

        var course = await _courseReadRepository.GetBySlugAsync(query.Slug);

        if (course is null)
            throw new NotFoundException("دوره پیدا نشد");

        return course.ToDto();
    }
}