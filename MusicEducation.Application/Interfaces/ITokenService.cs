using MusicEducation.Domain.Entities.Identity;

namespace MusicEducation.Application.Interfaces;

public interface ITokenService
{
    string GenerateAccessToken(User user);

    string GenerateRefreshToken();

    DateTime GetRefreshTokenExpiration();
}
