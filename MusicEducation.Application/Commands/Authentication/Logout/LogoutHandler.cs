using MusicEducation.Application.Exceptions;
using MusicEducation.Application.Interfaces;
using MusicEducation.Domain.Interfaces.Identity;

namespace MusicEducation.Application.Commands.Authentication.Logout;

public sealed class LogoutHandler
{
    private readonly IRefreshTokenRepository _refreshTokenRepository;
    private readonly IUnitOfWork _unitOfWork;

    public LogoutHandler(
        IRefreshTokenRepository refreshTokenRepository,
        IUnitOfWork unitOfWork)
    {
        _refreshTokenRepository = refreshTokenRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task Handle(
        LogoutCommand command,
        CancellationToken cancellationToken)
    {
        var refreshToken =
            await _refreshTokenRepository
                .GetByTokenAsync(command.RefreshToken);

        if (refreshToken is null)
        {
            throw new NotFoundException(
                "Refresh Token پیدا نشد");
        }

        if (refreshToken.IsRevoked)
        {
            return;
        }

        refreshToken.Revoke();

        await _refreshTokenRepository
            .UpdateAsync(refreshToken);

        await _unitOfWork.SaveChangesAsync(
            cancellationToken);
    }
}