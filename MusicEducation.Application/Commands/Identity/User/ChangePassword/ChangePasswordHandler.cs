using MusicEducation.Application.Exceptions;
using MusicEducation.Application.Interfaces;
using MusicEducation.Domain.Interfaces.Identity;

namespace MusicEducation.Application.Commands.Identity.User.ChangePassword;

public sealed class ChangePasswordHandler
{
    private readonly IUserRepository _userRepository;
    private readonly IPasswordHasher _passwordHasher;
    private readonly IUnitOfWork _unitOfWork;

    public ChangePasswordHandler(
        IUserRepository userRepository,
        IPasswordHasher passwordHasher,
        IUnitOfWork unitOfWork)
    {
        _userRepository = userRepository;
        _passwordHasher = passwordHasher;
        _unitOfWork = unitOfWork;
    }

    public async Task Handle(
        ChangePasswordCommand command,
        CancellationToken cancellationToken)
    {
        var user = await _userRepository.GetByIdAsync(command.UserId);

        if (user is null)
        {
            throw new NotFoundException("کاربر پیدا نشد");
        }

        if (!_passwordHasher.VerifyPassword(
                command.CurrentPassword,
                user.PasswordHash))
        {
            throw new UnauthorizedException("رمز عبور فعلی صحیح نیست");
        }

        var passwordHash =
            _passwordHasher.HashPassword(command.NewPassword);

        user.ChangePassword(passwordHash);

        await _userRepository.UpdateAsync(user);

        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }
}