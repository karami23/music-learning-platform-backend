using MusicEducation.Application.Exceptions;
using MusicEducation.Domain.Interfaces.Identity;
using MusicEducation.Application.Interfaces;

namespace MusicEducation.Application.Commands.Identity.Teacher.DeactivateTeacher;

public sealed class DeactivateTeacherHandler
{
    private readonly ITeacherRepository _teacherRepository;
    private readonly IUnitOfWork _unitOfWork;

    public DeactivateTeacherHandler(
        ITeacherRepository teacherRepository,
        IUnitOfWork unitOfWork)
    {
        _teacherRepository = teacherRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task Handle(
        DeactivateTeacherCommand command,
        CancellationToken cancellationToken)
    {
        var teacher = await _teacherRepository
            .GetByIdAsync(command.TeacherId);

        if (teacher is null)
        {
            throw new NotFoundException(
                "مدرس پیدا نشد");
        }

        teacher.Deactivate();

        await _teacherRepository
            .UpdateAsync(teacher);

        await _unitOfWork
            .SaveChangesAsync(cancellationToken);
    }
}