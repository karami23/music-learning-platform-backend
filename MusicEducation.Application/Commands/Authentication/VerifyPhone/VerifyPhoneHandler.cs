using MusicEducation.Application.Exceptions;
using MusicEducation.Application.Interfaces;
using MusicEducation.Domain.Enums;
using MusicEducation.Domain.Interfaces.Identity;
using MusicEducation.Domain.ValueObjects;

namespace MusicEducation.Application.Commands.Authentication.VerifyPhone;

public sealed class VerifyPhoneHandler
{
    private readonly IUserRepository _userRepository;
    private readonly IVerificationCodeRepository _verificationCodeRepository;
    private readonly IUnitOfWork _unitOfWork;

    public VerifyPhoneHandler(
        IUserRepository userRepository,
        IVerificationCodeRepository verificationCodeRepository,
        IUnitOfWork unitOfWork)
    {
        _userRepository = userRepository;
        _verificationCodeRepository = verificationCodeRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task Handle(
        VerifyPhoneCommand command,
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
                    VerificationCodePurpose.PhoneVerification);

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

        verificationCode.MarkAsUsed();

        user.VerifyPhone();

        await _verificationCodeRepository
            .UpdateAsync(verificationCode);

        await _userRepository
            .UpdateAsync(user);

        await _unitOfWork.SaveChangesAsync(
            cancellationToken);
    }
}