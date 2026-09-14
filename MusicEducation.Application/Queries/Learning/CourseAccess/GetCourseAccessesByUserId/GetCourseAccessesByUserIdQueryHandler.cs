using MusicEducation.Application.DTOs.Learning;
using MusicEducation.Application.Exceptions;
using MusicEducation.Application.Mappings.Learning;
using MusicEducation.Domain.Interfaces.Learning;

namespace MusicEducation.Application.Queries.Learning.CourseAccess.GetCourseAccessesByUserId;

public sealed class GetCourseAccessesByUserIdQueryHandler
{
    private readonly ICourseAccessRepository _courseAccessRepository;

    public GetCourseAccessesByUserIdQueryHandler(
        ICourseAccessRepository courseAccessRepository)
    {
        _courseAccessRepository = courseAccessRepository;
    }

    public async Task<IEnumerable<CourseAccessDto>> Handle(
        GetCourseAccessesByUserIdQuery query)
    {
        if (query.UserId <= 0)
            throw new ValidationException(
                "شناسه کاربر معتبر نیست");

        var courseAccesses =
            await _courseAccessRepository
                .GetByUserIdAsync(query.UserId);

        return courseAccesses
            .Select(x => x.ToDto())
            .ToList();
    }
}