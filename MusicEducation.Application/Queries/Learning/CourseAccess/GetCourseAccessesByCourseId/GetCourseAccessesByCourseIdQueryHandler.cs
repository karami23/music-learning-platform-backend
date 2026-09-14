using MusicEducation.Application.DTOs.Learning;
using MusicEducation.Application.Exceptions;
using MusicEducation.Application.Mappings.Learning;
using MusicEducation.Domain.Interfaces.Learning;

namespace MusicEducation.Application.Queries.Learning.CourseAccess.GetCourseAccessesByCourseId;

public sealed class GetCourseAccessesByCourseIdQueryHandler
{
    private readonly ICourseAccessRepository _courseAccessRepository;

    public GetCourseAccessesByCourseIdQueryHandler(
        ICourseAccessRepository courseAccessRepository)
    {
        _courseAccessRepository = courseAccessRepository;
    }

    public async Task<IEnumerable<CourseAccessDto>> Handle(
        GetCourseAccessesByCourseIdQuery query)
    {
        if (query.CourseId <= 0)
            throw new ValidationException(
                "شناسه دوره معتبر نیست");

        var courseAccesses =
            await _courseAccessRepository
                .GetByCourseIdAsync(query.CourseId);

        return courseAccesses
            .Select(x => x.ToDto())
            .ToList();
    }
}