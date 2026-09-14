using MusicEducation.Domain.Constants;
using MusicEducation.Domain.Exceptions;
using MusicEducation.Domain.Enums;

namespace MusicEducation.Domain.Entities.Identity;

public class Teacher : BaseEntity
{
    public int UserId { get; private set; }

    public string? Bio { get; private set; }

    public TeacherStatus Status { get; private set; }

    private Teacher() { }

    public static Teacher Create(
        int userId,
        string? bio = null)
    {
        ValidateUserId(userId);

        var normalizedBio = NormalizeBio(bio);

        return new Teacher
        {
            UserId = userId,
            Bio = normalizedBio,
            Status = TeacherStatus.Active
        };
    }

    public void UpdateProfile(string? bio)
    {
        Bio = NormalizeBio(bio);

        UpdatedAt = DateTime.UtcNow;
    }

    public void Activate()
    {
        if (Status == TeacherStatus.Active)
            return;

        Status = TeacherStatus.Active;
        UpdatedAt = DateTime.UtcNow;
    }

    public void Deactivate()
    {
        if (Status == TeacherStatus.Inactive)
            return;

        Status = TeacherStatus.Inactive;
        UpdatedAt = DateTime.UtcNow;
    }

    private static void ValidateUserId(int userId)
    {
        if (userId <= 0)
            throw new DomainException("شناسه کاربر معتبر نیست", nameof(userId));
    }

    private static string? NormalizeBio(string? bio)
    {
        if (string.IsNullOrWhiteSpace(bio))
            return null;

        var normalizedBio = bio.Trim();

        if (normalizedBio.Length > DomainConstants.MaxTeacherBioLength)
            throw new DomainException(
                $"توضیحات مدرس نمی‌تواند بیشتر از {DomainConstants.MaxTeacherBioLength} کاراکتر باشد",
                nameof(bio));

        return normalizedBio;
    }
}