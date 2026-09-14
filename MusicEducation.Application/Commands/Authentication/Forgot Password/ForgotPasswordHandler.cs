using MusicEducation.Application.Exceptions;
using MusicEducation.Application.Interfaces;
using MusicEducation.Domain.Entities.Identity;
using MusicEducation.Domain.Enums;
using MusicEducation.Domain.Interfaces.Identity;
using MusicEducation.Domain.ValueObjects;

namespace MusicEducation.Application.Commands.Authentication.ForgotPassword;

public sealed class ForgotPasswordHandler
{
    private readonly IUserRepository _userRepository;
    private readonly IVerificationCodeRepository _verificationCodeRepository;
    private readonly ISmsService _smsService;
    private readonly IUnitOfWork _unitOfWork;

    public ForgotPasswordHandler(
        IUserRepository userRepository,
        IVerificationCodeRepository verificationCodeRepository,
        ISmsService smsService,
        IUnitOfWork unitOfWork)
    {
        _userRepository = userRepository;
        _verificationCodeRepository = verificationCodeRepository;
        _smsService = smsService;
        _unitOfWork = unitOfWork;
    }

    public async Task Handle(
        ForgotPasswordCommand command,
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

        var existingCode =
            await _verificationCodeRepository
                .GetActiveByUserIdAndPurposeAsync(
                    user.Id,
                    VerificationCodePurpose.PasswordReset);

        if (existingCode is not null)
        {
            existingCode.Invalidate();

            await _verificationCodeRepository
                .UpdateAsync(existingCode);
        }

        var verificationCode = VerificationCode.Create(
            user.Id,
            GenerateVerificationCode(),
            DateTime.UtcNow.AddMinutes(2),
            VerificationCodePurpose.PasswordReset);

        await _verificationCodeRepository
            .AddAsync(verificationCode);

        await _unitOfWork.SaveChangesAsync(
            cancellationToken);

        await _smsService.SendVerificationCodeAsync(
            phoneNumber,
            verificationCode.Code);
    }

    private static string GenerateVerificationCode()
    {
        return Random.Shared
            .Next(100000, 1000000)
            .ToString();
    }
}