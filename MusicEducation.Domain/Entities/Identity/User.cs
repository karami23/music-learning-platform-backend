using MusicEducation.Domain.Constants;
using MusicEducation.Domain.Enums;
using MusicEducation.Domain.Exceptions;
using MusicEducation.Domain.ValueObjects;

namespace MusicEducation.Domain.Entities.Identity;

public class User : BaseEntity
{
    public Email Email { get; private set; } = null!;

    public string PasswordHash { get; private set; } = null!;

    public string FirstName { get; private set; } = null!;

    public string LastName { get; private set; } = null!;

    public PhoneNumber? PhoneNumber { get; private set; }

    public bool IsPhoneVerified { get; private set; }

    public string? ProfileImageUrl { get; private set; }

    public UserRole Role { get; private set; }

    public UserStatus Status { get; private set; }

    private User() { }
    public static User Create(
        Email email,
        string passwordHash,
        string firstName,
        string lastName,
        UserRole role,
        PhoneNumber? phoneNumber = null,
        string? profileImageUrl = null)
    {
        ValidatePasswordHash(passwordHash);
        ValidateRole(role);
        ValidateProfileImageUrl(profileImageUrl);

        return new User
        {
            Email = email,
            PasswordHash = passwordHash,
            FirstName = NormalizeFirstName(firstName),
            LastName = NormalizeLastName(lastName),
            PhoneNumber = phoneNumber,
            ProfileImageUrl = NormalizeProfileImageUrl(profileImageUrl),
            Role = role,
            IsPhoneVerified = false,
            Status = UserStatus.Active
        };
    }

    public void UpdateProfile(
        string firstName,
        string lastName,
        PhoneNumber? phoneNumber,
        string? profileImageUrl)
    {
        if (PhoneNumber != phoneNumber)
        {
            IsPhoneVerified = false;
        }

        FirstName = NormalizeFirstName(firstName);
        LastName = NormalizeLastName(lastName);
        PhoneNumber = phoneNumber;
        ProfileImageUrl = NormalizeProfileImageUrl(profileImageUrl);
        UpdatedAt = DateTime.UtcNow;
    }
    public void ChangePassword(string passwordHash)
    {
        ValidatePasswordHash(passwordHash);

        PasswordHash = passwordHash;

        UpdatedAt = DateTime.UtcNow;
    }

    public void PromoteToTeacher()
    {
        if (Role == UserRole.Teacher)
            throw new DomainException("این کاربر قبلاً مدرس است");

        Role = UserRole.Teacher;
        UpdatedAt = DateTime.UtcNow;
    }
    public void Activate()
    {
        if (Status == UserStatus.Active)
            return;

        Status = UserStatus.Active;
        UpdatedAt = DateTime.UtcNow;
    }

    public void Deactivate()
    {
        if (Status == UserStatus.Inactive)
            return;

        Status = UserStatus.Inactive;
        UpdatedAt = DateTime.UtcNow;
    }

    private static void ValidatePasswordHash(string passwordHash)
    {
        if (string.IsNullOrWhiteSpace(passwordHash))
            throw new DomainException("هش رمز عبور الزامی است", nameof(passwordHash));
    }

    private static void ValidateRole(UserRole role)
    {
        if (!Enum.IsDefined(role))
            throw new DomainException("نقش کاربر معتبر نیست", nameof(role));
    }

    private static string NormalizeFirstName(string firstName)
    {
        if (string.IsNullOrWhiteSpace(firstName))
            throw new DomainException("نام الزامی است", nameof(firstName));

        var normalized = firstName.Trim();

        if (normalized.Length > DomainConstants.MaxUserFirstNameLength)
            throw new DomainException(
                $"نام نمی‌تواند بیشتر از {DomainConstants.MaxUserFirstNameLength} کاراکتر باشد",
                nameof(firstName));

        return normalized;
    }

    private static string NormalizeLastName(string lastName)
    {
        if (string.IsNullOrWhiteSpace(lastName))
            throw new DomainException("نام خانوادگی الزامی است",
                nameof(lastName));

        var normalized = lastName.Trim();

        if (normalized.Length > DomainConstants.MaxUserLastNameLength)
            throw new DomainException(
                $"نام خانوادگی نمی‌تواند بیشتر از {DomainConstants.MaxUserLastNameLength} کاراکتر باشد",
                nameof(lastName));

        return normalized;
    }

    private static string? NormalizeProfileImageUrl(string? profileImageUrl)
    {
        if (string.IsNullOrWhiteSpace(profileImageUrl))
            return null;

        var normalized = profileImageUrl.Trim();

        ValidateProfileImageUrl(normalized);

        return normalized;
    }

    private static void ValidateProfileImageUrl(string? profileImageUrl)
    {
        if (string.IsNullOrWhiteSpace(profileImageUrl))
            return;

        if (profileImageUrl.Length > DomainConstants.MaxProfileImageUrlLength)
            throw new DomainException(
                $"آدرس تصویر پروفایل نمی‌تواند بیشتر از {DomainConstants.MaxProfileImageUrlLength} کاراکتر باشد",
                nameof(profileImageUrl));

        if (!Uri.TryCreate(
                profileImageUrl,
                UriKind.Absolute,
                out var uri) ||
            (uri.Scheme != Uri.UriSchemeHttp &&
             uri.Scheme != Uri.UriSchemeHttps))
        {
            throw new DomainException("آدرس تصویر پروفایل معتبر نیست", nameof(profileImageUrl));
        }
    }
    public void VerifyPhone()
    {
        if (PhoneNumber is null)
            throw new DomainException("کاربر شماره تلفن ندارد");

        if (IsPhoneVerified)
            return;

        IsPhoneVerified = true;
        UpdatedAt = DateTime.UtcNow;
    }
}