using MusicEducation.Application.DTOs.Learning;
using MusicEducation.Application.Mappings.Learning;
using MusicEducation.Application.Exceptions;
using MusicEducation.Domain.Interfaces.Learning;

namespace MusicEducation.Application.Queries.Learning.CourseAccess.GetCourseAccessById;

public sealed class GetCourseAccessByIdQueryHandler
{
    private readonly ICourseAccessRepository _courseAccessRepository;

    public GetCourseAccessByIdQueryHandler(
        ICourseAccessRepository courseAccessRepository)
    {
        _courseAccessRepository = courseAccessRepository;
    }

    public async Task<CourseAccessDto?> Handle(
        GetCourseAccessByIdQuery query)
    {
        if (query.CourseAccessId <= 0)
            throw new ValidationException(
                "شناسه دسترسی دوره معتبر نیست");

        var courseAccess = await _courseAccessRepository
            .GetByIdAsync(query.CourseAccessId);

        return courseAccess?.ToDto();
    }
}