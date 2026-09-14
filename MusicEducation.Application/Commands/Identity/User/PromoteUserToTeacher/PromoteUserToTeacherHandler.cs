using MusicEducation.Application.Exceptions;
using MusicEducation.Application.Interfaces;
using TeacherEntity = MusicEducation.Domain.Entities.Identity.Teacher;
using MusicEducation.Domain.Enums;
using MusicEducation.Domain.Interfaces.Identity;

namespace MusicEducation.Application.Commands.Identity.User.PromoteUserToTeacher;

public sealed class PromoteUserToTeacherHandler
{
    private readonly IUserRepository _userRepository;
    private readonly ITeacherRepository _teacherRepository;
    private readonly IUnitOfWork _unitOfWork;

    public PromoteUserToTeacherHandler(
        IUserRepository userRepository,
        ITeacherRepository teacherRepository,
        IUnitOfWork unitOfWork)
    {
        _userRepository = userRepository;
        _teacherRepository = teacherRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task Handle(
        PromoteUserToTeacherCommand command,
        CancellationToken cancellationToken)
    {
        var user = await _userRepository.GetByIdAsync(
            command.UserId);

        if (user is null)
        {
            throw new NotFoundException("کاربر پیدا نشد");
        }

        if (user.Role == UserRole.Teacher)
        {
            throw new ConflictException("این کاربر قبلاً مدرس است");
        }

        if (await _teacherRepository.ExistsByUserIdAsync(user.Id))
        {
            throw new ConflictException("برای این کاربر قبلاً پروفایل مدرس ایجاد شده است");
        }

        user.PromoteToTeacher();

        var teacher = TeacherEntity.Create(
            user.Id,
            command.Bio);

        await _userRepository.UpdateAsync(user);
        await _teacherRepository.AddAsync(teacher);

        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }
}