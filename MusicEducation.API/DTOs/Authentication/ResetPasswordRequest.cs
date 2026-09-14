namespace MusicEducation.API.DTOs.Authentication;

public sealed record ResetPasswordRequest(
    string CountryCode,
    string NationalNumber,
    string VerificationCode,
    string NewPassword
);