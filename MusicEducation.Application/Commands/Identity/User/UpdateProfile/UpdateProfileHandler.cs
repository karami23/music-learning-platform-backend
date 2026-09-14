using MusicEducation.Application.Exceptions;
using MusicEducation.Application.Interfaces;
using MusicEducation.Domain.Entities.Identity;
using MusicEducation.Domain.Enums;
using MusicEducation.Domain.Interfaces.Identity;
using MusicEducation.Domain.ValueObjects;

namespace MusicEducation.Application.Commands.Identity.User.UpdateProfile;

public sealed class UpdateProfileHandler
{
    private readonly IUserRepository _userRepository;
    private readonly IVerificationCodeRepository _verificationCodeRepository;
    private readonly ISmsService _smsService;
    private readonly IUnitOfWork _unitOfWork;

    public UpdateProfileHandler(
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
        UpdateProfileCommand command,
        CancellationToken cancellationToken)
    {
        var user = await _userRepository
            .GetByIdAsync(command.UserId);

        if (user is null)
        {
            throw new NotFoundException(
                "کاربر پیدا نشد");
        }

        PhoneNumber? phoneNumber = null;

        var hasCountryCode =
            !string.IsNullOrWhiteSpace(command.CountryCode);

        var hasNationalNumber =
            !string.IsNullOrWhiteSpace(command.NationalNumber);

        if (hasCountryCode != hasNationalNumber)
        {
            throw new ValidationException(
                "برای ثبت شماره تلفن، کد کشور و شماره تلفن هر دو الزامی هستند");
        }

        var phoneChanged = false;

        if (hasCountryCode && hasNationalNumber)
        {
            phoneNumber = PhoneNumber.Create(
                command.CountryCode!,
                command.NationalNumber!);

            phoneChanged = user.PhoneNumber != phoneNumber;

            if (phoneChanged &&
                await _userRepository
                    .ExistsByPhoneNumberAsync(phoneNumber))
            {
                throw new ConflictException(
                    "این شماره تلفن قبلاً ثبت شده است");
            }
        }
        else if (user.PhoneNumber is not null)
        {
            phoneChanged = true;
        }

        user.UpdateProfile(
            command.FirstName,
            command.LastName,
            phoneNumber,
            command.ProfileImageUrl);

        await _userRepository
            .UpdateAsync(user);

        VerificationCode? verificationCode = null;

        if (phoneChanged && phoneNumber is not null)
        {
            var existingCode =
                await _verificationCodeRepository
                    .GetActiveByUserIdAndPurposeAsync(
                        user.Id,
                        VerificationCodePurpose.PhoneVerification);

            if (existingCode is not null)
            {
                existingCode.Invalidate();

                await _verificationCodeRepository
                    .UpdateAsync(existingCode);
            }

            verificationCode = VerificationCode.Create(
                user.Id,
                GenerateVerificationCode(),
                DateTime.UtcNow.AddMinutes(5),
                VerificationCodePurpose.PhoneVerification);

            await _verificationCodeRepository
                .AddAsync(verificationCode);
        }

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        if (verificationCode is not null)
        {
            await _smsService.SendVerificationCodeAsync(
                phoneNumber!,
                verificationCode.Code);
        }
    }

    private static string GenerateVerificationCode()
    {
        return Random.Shared
            .Next(100000, 1000000)
            .ToString();
    }
}