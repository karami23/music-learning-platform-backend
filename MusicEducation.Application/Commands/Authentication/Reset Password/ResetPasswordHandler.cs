using MusicEducation.Application.Exceptions;
using MusicEducation.Application.Interfaces;
using MusicEducation.Domain.Enums;
using MusicEducation.Domain.Interfaces.Identity;
using MusicEducation.Domain.ValueObjects;

namespace MusicEducation.Application.Commands.Authentication.ResetPassword;

public sealed class ResetPasswordHandler
{
    private readonly IUserRepository _userRepository;
    private readonly IVerificationCodeRepository _verificationCodeRepository;
    private readonly IPasswordHasher _passwordHasher;
    private readonly IUnitOfWork _unitOfWork;

    public ResetPasswordHandler(
        IUserRepository userRepository,
        IVerificationCodeRepository verificationCodeRepository,
        IPasswordHasher passwordHasher,
        IUnitOfWork unitOfWork)
    {
        _userRepository = userRepository;
        _verificationCodeRepository = verificationCodeRepository;
        _passwordHasher = passwordHasher;
        _unitOfWork = unitOfWork;
    }

    public async Task Handle(
        ResetPasswordCommand command,
        CancellationToken cancellationToken)
    {
        var phoneNumber = PhoneNumber.Create(
            command.CountryCode,
            command.NationalNumber);

        var user = await _userRepository
            .GetByPhoneNumberAsync(phoneNumber);

        if (user is null)
        {
            throw new NotFoundException(
                "کاربری با این شماره تلفن پیدا نشد");
        }

        var verificationCode =
            await _verificationCodeRepository
                .GetActiveByUserIdAndPurposeAsync(
                    user.Id,
                    VerificationCodePurpose.PasswordReset);

        if (verificationCode is null)
        {
            throw new UnauthorizedException(
                "کد تأیید معتبر نیست یا منقضی شده است");
        }

        if (verificationCode.Code !=
            command.VerificationCode)
        {
            throw new UnauthorizedException(
                "کد تأیید صحیح نیست");
        }

        var passwordHash =
            _passwordHasher.HashPassword(
                command.NewPassword);

        user.ChangePassword(passwordHash);

        verificationCode.MarkAsUsed();

        await _userRepository
            .UpdateAsync(user);

        await _verificationCodeRepository
            .UpdateAsync(verificationCode);

        await _unitOfWork.SaveChangesAsync(
            cancellationToken);
    }
}