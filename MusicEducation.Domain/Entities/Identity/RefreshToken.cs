namespace MusicEducation.Domain.Entities.Identity;

public class RefreshToken : BaseEntity
{
    public int UserId { get; private set; }

    public string Token { get; private set; } = null!;

    public DateTime ExpiresAt { get; private set; }

    public DateTime? RevokedAt { get; private set; }

    public bool IsExpired => DateTime.UtcNow >= ExpiresAt;

    public bool IsRevoked => RevokedAt.HasValue;

    public bool IsActive => !IsExpired && !IsRevoked;

    private RefreshToken() { }

    public static RefreshToken Create(
        int userId,
        string token,
        DateTime expiresAt)
    {
        if (userId <= 0)
            throw new ArgumentException("شناسه کاربر معتبر نیست",
                nameof(userId));

        if (string.IsNullOrWhiteSpace(token))
            throw new ArgumentException("Refresh Token الزامی است",
                nameof(token));

        if (expiresAt <= DateTime.UtcNow)
            throw new ArgumentException("زمان انقضای Refresh Token باید در آینده باشد",
                nameof(expiresAt));

        return new RefreshToken
        {
            UserId = userId,
            Token = token,
            ExpiresAt = expiresAt
        };
    }

    public void Revoke()
    {
        if (IsRevoked)
            return;

        RevokedAt = DateTime.UtcNow;
        UpdatedAt = DateTime.UtcNow;
    }
}