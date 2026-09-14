using MusicEducation.Application.DTOs.Learning;
using MusicEducation.Application.Exceptions;
using MusicEducation.Application.Mappings.Learning;
using MusicEducation.Domain.Interfaces.Learning;

namespace MusicEducation.Application.Queries.Learning.CourseAccess.GetCourseAccessByUserAndCourse;

public sealed class GetCourseAccessByUserAndCourseQueryHandler
{
    private readonly ICourseAccessRepository _courseAccessRepository;

    public GetCourseAccessByUserAndCourseQueryHandler(
        ICourseAccessRepository courseAccessRepository)
    {
        _courseAccessRepository = courseAccessRepository;
    }

    public async Task<CourseAccessDto?> Handle(
        GetCourseAccessByUserAndCourseQuery query)
    {
        if (query.UserId <= 0)
            throw new ValidationException(
                "شناسه کاربر معتبر نیست");

        if (query.CourseId <= 0)
            throw new ValidationException(
                "شناسه دوره معتبر نیست");

        var courseAccess =
            await _courseAccessRepository
                .GetByUserIdAndCourseIdAsync(
                    query.UserId,
                    query.CourseId);

        return courseAccess?.ToDto();
    }
}
