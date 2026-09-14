using MusicEducation.Application.DTOs.Identity;
using MusicEducation.Application.Mappings.Identity;
using MusicEducation.Domain.Interfaces.Identity;

namespace MusicEducation.Application.Queries.Identity.Teacher.GetAllTeachers;

public sealed class GetAllTeachersHandler
{
    private readonly ITeacherRepository _teacherRepository;

    public GetAllTeachersHandler(ITeacherRepository teacherRepository)
    {
        _teacherRepository = teacherRepository;
    }

    public async Task<IEnumerable<TeacherDto>> Handle(GetAllTeachersQuery query)
    {
        var teachers = await _teacherRepository.GetAllAsync();

        return teachers.Select(teacher => teacher.ToDto());
    }
}