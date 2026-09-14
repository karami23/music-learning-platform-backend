using MusicEducation.Domain.Enums;
using MusicEducation.Domain.Exceptions;

namespace MusicEducation.Domain.Entities.Identity;

public class VerificationCode : BaseEntity
{
    public int UserId { get; private set; }

    public string Code { get; private set; } = null!;

    public DateTime ExpiresAt { get; private set; }

    public DateTime? UsedAt { get; private set; }
    public VerificationCodePurpose Purpose { get; private set; }

    public bool IsExpired => DateTime.UtcNow >= ExpiresAt;

    public bool IsUsed => UsedAt.HasValue;

    public bool IsActive => !IsExpired && !IsUsed;

    private VerificationCode() { }

    public static VerificationCode Create(
      int userId,
      string code,
      DateTime expiresAt,
      VerificationCodePurpose purpose)
    {
        if (userId <= 0)
            throw new DomainException("شناسه کاربر معتبر نیست",
                nameof(userId));

        if (string.IsNullOrWhiteSpace(code))
            throw new DomainException("کد تأیید الزامی است",
                nameof(code));

        if (expiresAt <= DateTime.UtcNow)
            throw new DomainException("زمان انقضای کد تأیید باید در آینده باشد",
                nameof(expiresAt));

        if (!Enum.IsDefined(purpose))
            throw new DomainException("نوع کد تأیید معتبر نیست",
                nameof(purpose));

        return new VerificationCode
        {
            UserId = userId,
            Code = code,
            ExpiresAt = expiresAt,
            Purpose = purpose
        };
    }

    public void MarkAsUsed()
    {
        if (IsUsed)
            return;

        UsedAt = DateTime.UtcNow;
        UpdatedAt = DateTime.UtcNow;
    }
    public void Invalidate()
    {
        if (IsUsed)
            return;

        UsedAt = DateTime.UtcNow;
        UpdatedAt = DateTime.UtcNow;
    }
}