using MusicEducation.Application.Exceptions;
using MusicEducation.Application.Interfaces;
using MusicEducation.Domain.Entities.Identity;
using MusicEducation.Domain.Enums;
using MusicEducation.Domain.Interfaces.Identity;
using MusicEducation.Domain.ValueObjects;

namespace MusicEducation.Application.Commands.Authentication.Register;

public sealed class RegisterUserHandler
{
    private readonly IUserRepository _userRepository;
    private readonly IPasswordHasher _passwordHasher;
    private readonly IUnitOfWork _unitOfWork;

    public RegisterUserHandler(
        IUserRepository userRepository,
        IPasswordHasher passwordHasher,
        IUnitOfWork unitOfWork)
    {
        _userRepository = userRepository;
        _passwordHasher = passwordHasher;
        _unitOfWork = unitOfWork;
    }

    public async Task Handle(
        RegisterUserCommand command,
        CancellationToken cancellationToken)
    {
        var email = Email.Create(command.Email);

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

        if (hasCountryCode && hasNationalNumber)
        {
            phoneNumber = PhoneNumber.Create(
                command.CountryCode!,
                command.NationalNumber!);
        }

        if (await _userRepository
                .ExistsByEmailAsync(email))
        {
            throw new ConflictException(
                "این ایمیل قبلاً ثبت شده است");
        }

        if (phoneNumber is not null &&
            await _userRepository
                .ExistsByPhoneNumberAsync(phoneNumber))
        {
            throw new ConflictException(
                "این شماره تلفن قبلاً ثبت شده است");
        }

        var passwordHash =
            _passwordHasher.HashPassword(
                command.Password);

        var user = User.Create(
            email,
            passwordHash,
            command.FirstName,
            command.LastName,
            UserRole.Student,
            phoneNumber);

        await _userRepository.AddAsync(user);

        await _unitOfWork.SaveChangesAsync(
            cancellationToken);
    }
}