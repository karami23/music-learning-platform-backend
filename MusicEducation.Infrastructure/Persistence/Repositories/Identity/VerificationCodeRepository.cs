using Microsoft.EntityFrameworkCore;
using MusicEducation.Domain.Entities.Identity;
using MusicEducation.Domain.Enums;
using MusicEducation.Domain.Interfaces.Identity;
using MusicEducation.Infrastructure.Persistence.DbContext;

namespace MusicEducation.Infrastructure.Repositories.Identity;

public class VerificationCodeRepository : IVerificationCodeRepository
{
    private readonly MusicEducationDbContext _context;

    public VerificationCodeRepository(MusicEducationDbContext context)
    {
        _context = context;
    }

    public async Task<VerificationCode?> GetActiveByUserIdAndPurposeAsync(
        int userId,
        VerificationCodePurpose purpose)
    {
        if (userId <= 0)
            throw new ArgumentException(
                "شناسه کاربر معتبر نیست.",
                nameof(userId));

        return await _context.Set<VerificationCode>()
            .FirstOrDefaultAsync(x =>
                x.UserId == userId &&
                x.Purpose == purpose &&
                x.UsedAt == null &&
                x.ExpiresAt > DateTime.UtcNow);
    }

    public async Task<VerificationCode> AddAsync(
        VerificationCode verificationCode)
    {
        ArgumentNullException.ThrowIfNull(verificationCode);

        await _context.Set<VerificationCode>()
            .AddAsync(verificationCode);

        return verificationCode;
    }

    public Task UpdateAsync(VerificationCode verificationCode)
    {
        ArgumentNullException.ThrowIfNull(verificationCode);

        _context.Set<VerificationCode>()
            .Update(verificationCode);

        return Task.CompletedTask;
    }
}