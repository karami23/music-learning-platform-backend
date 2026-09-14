using MusicEducation.Application.DTOs.Identity;
using MusicEducation.Application.Exceptions;
using MusicEducation.Application.Mappings.Identity;
using MusicEducation.Domain.Interfaces.Identity;

namespace MusicEducation.Application.Queries.Identity.Teacher.GetTeacherByUserId;

public sealed class GetTeacherByUserIdHandler
{
    private readonly ITeacherRepository _teacherRepository;

    public GetTeacherByUserIdHandler(ITeacherRepository teacherRepository)
    {
        _teacherRepository = teacherRepository;
    }

    public async Task<TeacherDto> Handle(GetTeacherByUserIdQuery query)
    {
        var teacher = await _teacherRepository.GetByUserIdAsync(query.UserId);

        if (teacher is null)
        {
            throw new NotFoundException("مدرس مربوط به این کاربر پیدا نشد");
        }

        return teacher.ToDto();
    }
}