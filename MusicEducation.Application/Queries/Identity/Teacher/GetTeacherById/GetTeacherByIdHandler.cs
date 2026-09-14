using MusicEducation.Application.DTOs.Identity;
using MusicEducation.Application.Exceptions;
using MusicEducation.Application.Mappings.Identity;
using MusicEducation.Domain.Interfaces.Identity;

namespace MusicEducation.Application.Queries.Identity.Teacher.GetTeacherById;

public sealed class GetTeacherByIdHandler
{
    private readonly ITeacherRepository _teacherRepository;

    public GetTeacherByIdHandler(ITeacherRepository teacherRepository)
    {
        _teacherRepository = teacherRepository;
    }

    public async Task<TeacherDto> Handle(GetTeacherByIdQuery query)
    {
        var teacher = await _teacherRepository.GetByIdAsync(query.TeacherId);

        if (teacher is null)
        {
            throw new NotFoundException("مدرس پیدا نشد");
        }

        return teacher.ToDto();
    }
}