using MusicEducation.Domain.Entities.Identity;

namespace MusicEducation.Domain.Interfaces.Identity;

public interface IRefreshTokenRepository
{
    Task<RefreshToken?> GetByTokenAsync(string token);

    Task<IEnumerable<RefreshToken>> GetByUserIdAsync(int userId);

    Task<RefreshToken> AddAsync(RefreshToken refreshToken);

    Task UpdateAsync(RefreshToken refreshToken);
}