using Microsoft.EntityFrameworkCore;
using MusicEducation.Domain.Entities.Identity;
using MusicEducation.Domain.Interfaces.Identity;
using MusicEducation.Infrastructure.Persistence.DbContext;

namespace MusicEducation.Infrastructure.Repositories.Identity;

public class RefreshTokenRepository : IRefreshTokenRepository
{
    private readonly MusicEducationDbContext _context;

    public RefreshTokenRepository(MusicEducationDbContext context)
    {
        _context = context;
    }

    public async Task<RefreshToken?> GetByTokenAsync(string token)
    {
        if (string.IsNullOrWhiteSpace(token))
            return null;

        return await _context.Set<RefreshToken>()
            .FirstOrDefaultAsync(x => x.Token == token);
    }

    public async Task<IEnumerable<RefreshToken>> GetByUserIdAsync(int userId)
    {
        return await _context.Set<RefreshToken>()
            .Where(x => x.UserId == userId)
            .ToListAsync();
    }

    public async Task<RefreshToken> AddAsync(RefreshToken refreshToken)
    {
        ArgumentNullException.ThrowIfNull(refreshToken);

        await _context.Set<RefreshToken>().AddAsync(refreshToken);

        return refreshToken;
    }

    public Task UpdateAsync(RefreshToken refreshToken)
    {
        ArgumentNullException.ThrowIfNull(refreshToken);

        _context.Set<RefreshToken>().Update(refreshToken);

        return Task.CompletedTask;
    }
}