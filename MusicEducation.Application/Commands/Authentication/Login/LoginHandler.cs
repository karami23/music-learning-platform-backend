using MusicEducation.Application.Commands.Authentication.LoginType;
using MusicEducation.Application.DTOs.Authentication;
using MusicEducation.Application.Exceptions;
using MusicEducation.Application.Interfaces;
using MusicEducation.Domain.Entities.Identity;
using MusicEducation.Domain.Enums;
using MusicEducation.Domain.Exceptions;
using MusicEducation.Domain.Entities.Identity;
using MusicEducation.Domain.Interfaces.Identity;
using MusicEducation.Domain.ValueObjects;

namespace MusicEducation.Application.Commands.Authentication.Login;

public sealed class LoginHandler
{
    private readonly IUserRepository _userRepository;
    private readonly IRefreshTokenRepository _refreshTokenRepository;
    private readonly IPasswordHasher _passwordHasher;
    private readonly ITokenService _tokenService;
    private readonly IUnitOfWork _unitOfWork;

    public LoginHandler(
        IUserRepository userRepository,
        IRefreshTokenRepository refreshTokenRepository,
        IPasswordHasher passwordHasher,
        ITokenService tokenService,
        IUnitOfWork unitOfWork)
    {
        _userRepository = userRepository;
        _refreshTokenRepository = refreshTokenRepository;
        _passwordHasher = passwordHasher;
        _tokenService = tokenService;
        _unitOfWork = unitOfWork;
    }

    public async Task<AuthenticationResponse> Handle(
        LoginCommand command,
        CancellationToken cancellationToken)
    {
        User? user;

        switch (command.LoginType)
        {
            case LoginType.LoginType.Email:

                if (string.IsNullOrWhiteSpace(command.Email))
                {
                    throw new ValidationException(
                        "برای ورود با ایمیل، ایمیل الزامی است");
                }

                if (string.IsNullOrWhiteSpace(command.Password))
                {
                    throw new ValidationException(
                        "رمز عبور الزامی است");
                }

                user = await GetUserByEmailAsync(
                    command.Email);

                break;

            case LoginType.LoginType.Phone:

                if (string.IsNullOrWhiteSpace(command.CountryCode) ||
                    string.IsNullOrWhiteSpace(command.NationalNumber))
                {
                    throw new ValidationException(
                        "برای ورود با شماره تلفن، کد کشور و شماره تلفن هر دو الزامی هستند");
                }

                if (string.IsNullOrWhiteSpace(command.Password))
                {
                    throw new ValidationException(
                        "رمز عبور الزامی است");
                }

                user = await GetUserByPhoneAsync(
                    command.CountryCode,
                    command.NationalNumber);

                break;

            default:

                throw new ValidationException(
                    "روش ورود معتبر نیست");
        }

        if (user is null)
        {
            throw new UnauthorizedException(
                "ایمیل، شماره تلفن یا رمز عبور صحیح نیست");
        }

        if (user.Status != UserStatus.Active)
        {
            throw new UnauthorizedException(
                "حساب کاربری شما فعال نیست");
        }

        if (command.LoginType == LoginType.LoginType.Phone &&
            !user.IsPhoneVerified)
        {
            throw new UnauthorizedException(
                "شماره تلفن شما تأیید نشده است");
        }

        if (!_passwordHasher.VerifyPassword(
                command.Password,
                user.PasswordHash))
        {
            throw new UnauthorizedException(
                "ایمیل، شماره تلفن یا رمز عبور صحیح نیست");
        }

        var accessToken =
            _tokenService.GenerateAccessToken(user);

        var refreshToken =
            _tokenService.GenerateRefreshToken();

        var refreshTokenEntity =
            MusicEducation.Domain.Entities.Identity.RefreshToken.Create(
                user.Id,
                refreshToken,
                _tokenService.GetRefreshTokenExpiration());

        await _refreshTokenRepository
            .AddAsync(refreshTokenEntity);

        await _unitOfWork.SaveChangesAsync(
            cancellationToken);

        return new AuthenticationResponse(
            accessToken,
            refreshToken);
    }

    private async Task<User?> GetUserByEmailAsync(
        string emailValue)
    {
        var email = Email.Create(emailValue);

        return await _userRepository
            .GetByEmailAsync(email);
    }

    private async Task<User?> GetUserByPhoneAsync(
        string countryCode,
        string nationalNumber)
    {
        var phoneNumber = PhoneNumber.Create(
            countryCode,
            nationalNumber);

        return await _userRepository
            .GetByPhoneNumberAsync(phoneNumber);
    }
}