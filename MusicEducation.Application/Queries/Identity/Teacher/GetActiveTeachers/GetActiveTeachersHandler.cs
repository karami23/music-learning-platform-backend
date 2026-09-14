using MusicEducation.Application.DTOs.Identity;
using MusicEducation.Application.Mappings.Identity;
using MusicEducation.Domain.Interfaces.Identity;

namespace MusicEducation.Application.Queries.Identity.Teacher.GetActiveTeachers;

public sealed class GetActiveTeachersHandler
{
    private readonly ITeacherRepository _teacherRepository;

    public GetActiveTeachersHandler(ITeacherRepository teacherRepository)
    {
        _teacherRepository = teacherRepository;
    }

    public async Task<IEnumerable<TeacherDto>> Handle(GetActiveTeachersQuery query)
    {
        var teachers = await _teacherRepository.GetActiveTeachersAsync();

        return teachers.Select(teacher => teacher.ToDto());
    }
}