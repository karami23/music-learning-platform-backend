using MusicEducation.Application.DTOs.Learning;
using MusicEducation.Domain.Entities.Learning;

namespace MusicEducation.Application.Mappings.Learning;

public static class CourseAccessMapping
{
    public static CourseAccessDto ToDto(this CourseAccess courseAccess)
    {
        return new CourseAccessDto(
            courseAccess.Id,
            courseAccess.UserId,
            courseAccess.CourseId,
            courseAccess.Status,
            courseAccess.GrantedAt,
            courseAccess.RevokedAt
        );
    }
}