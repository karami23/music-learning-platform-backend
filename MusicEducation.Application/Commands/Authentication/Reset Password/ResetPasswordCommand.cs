namespace MusicEducation.Application.Commands.Authentication.ResetPassword;

public sealed record ResetPasswordCommand(
    string CountryCode,
    string NationalNumber,
    string VerificationCode,
    string NewPassword
);