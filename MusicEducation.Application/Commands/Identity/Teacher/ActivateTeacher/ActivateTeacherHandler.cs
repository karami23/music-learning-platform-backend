using MusicEducation.Application.Exceptions;
using MusicEducation.Application.Interfaces;
using MusicEducation.Domain.Interfaces.Identity;

namespace MusicEducation.Application.Commands.Identity.Teacher.ActivateTeacher;

public sealed class ActivateTeacherHandler
{
    private readonly ITeacherRepository _teacherRepository;
    private readonly IUnitOfWork _unitOfWork;

    public ActivateTeacherHandler(
        ITeacherRepository teacherRepository,
        IUnitOfWork unitOfWork)
    {
        _teacherRepository = teacherRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task Handle(
        ActivateTeacherCommand command,
        CancellationToken cancellationToken)
    {
        var teacher = await _teacherRepository
            .GetByIdAsync(command.TeacherId);

        if (teacher is null)
        {
            throw new NotFoundException(
                "مدرس پیدا نشد");
        }

        teacher.Activate();

        await _teacherRepository
            .UpdateAsync(teacher);

        await _unitOfWork
            .SaveChangesAsync(cancellationToken);
    }
}