using MusicEducation.Application.DTOs.Authentication;
using MusicEducation.Application.Exceptions;
using MusicEducation.Application.Interfaces;
using MusicEducation.Domain.Interfaces.Identity;
using MusicEducation.Domain.Enums;
using RefreshTokenEntity =
    MusicEducation.Domain.Entities.Identity.RefreshToken;

namespace MusicEducation.Application.Commands.Authentication.RefreshToken;

public sealed class RefreshTokenHandler
{
    private readonly IRefreshTokenRepository _refreshTokenRepository;
    private readonly IUserRepository _userRepository;
    private readonly ITokenService _tokenService;
    private readonly IUnitOfWork _unitOfWork;

    public RefreshTokenHandler(
        IRefreshTokenRepository refreshTokenRepository,
        IUserRepository userRepository,
        ITokenService tokenService,
        IUnitOfWork unitOfWork)
    {
        _refreshTokenRepository = refreshTokenRepository;
        _userRepository = userRepository;
        _tokenService = tokenService;
        _unitOfWork = unitOfWork;
    }

    public async Task<AuthenticationResponse> Handle(
        RefreshTokenCommand command,
        CancellationToken cancellationToken)
    {
        var refreshToken =
            await _refreshTokenRepository
                .GetByTokenAsync(command.RefreshToken);

        if (refreshToken is null ||
            !refreshToken.IsActive)
        {
            throw new UnauthorizedException(
                "Refresh Token معتبر نیست");
        }

        var user = await _userRepository
            .GetByIdAsync(refreshToken.UserId);

        if (user is null)
        {
            throw new UnauthorizedException(
                "کاربر معتبر نیست");
        }

        if (user.Status != UserStatus.Active)
        {
            throw new UnauthorizedException(
                "حساب کاربری شما فعال نیست");
        }

        refreshToken.Revoke();

        await _refreshTokenRepository
            .UpdateAsync(refreshToken);

        var accessToken =
            _tokenService.GenerateAccessToken(user);

        var newRefreshToken =
            _tokenService.GenerateRefreshToken();

        var newRefreshTokenEntity =
            RefreshTokenEntity.Create(
                user.Id,
                newRefreshToken,
                _tokenService.GetRefreshTokenExpiration());

        await _refreshTokenRepository
            .AddAsync(newRefreshTokenEntity);

        await _unitOfWork.SaveChangesAsync(
            cancellationToken);

        return new AuthenticationResponse(
            accessToken,
            newRefreshToken);
    }
}