using MusicEducation.Application.Exceptions;
using MusicEducation.Application.Interfaces;
using MusicEducation.Domain.Interfaces.Identity;

namespace MusicEducation.Application.Commands.Identity.Teacher.UpdateProfile;

public sealed class UpdateTeacherProfileHandler
{
    private readonly ITeacherRepository _teacherRepository;
    private readonly IUnitOfWork _unitOfWork;

    public UpdateTeacherProfileHandler(
        ITeacherRepository teacherRepository,
        IUnitOfWork unitOfWork)
    {
        _teacherRepository = teacherRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task Handle(
        UpdateTeacherProfileCommand command,
        CancellationToken cancellationToken)
    {
        var teacher = await _teacherRepository
            .GetByUserIdAsync(command.UserId);

        if (teacher is null)
        {
            throw new NotFoundException("مدرس پیدا نشد");
        }

        teacher.UpdateProfile(command.Bio);

        await _teacherRepository
            .UpdateAsync(teacher);

        await _unitOfWork
            .SaveChangesAsync(cancellationToken);
    }
}