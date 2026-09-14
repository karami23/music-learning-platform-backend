using MusicEducation.Application.DTOs.Identity;
using MusicEducation.Domain.Entities.Identity;

namespace MusicEducation.Application.Mappings.Identity;

public static class TeacherMapping
{
    public static TeacherDto ToDto(this Teacher teacher)
    {
        return new TeacherDto(
            teacher.Id,
            teacher.UserId,
            teacher.Bio,
            teacher.Status
        );
    }
}