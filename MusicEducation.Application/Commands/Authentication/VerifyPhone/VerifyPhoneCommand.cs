namespace MusicEducation.Application.Commands.Authentication.VerifyPhone;

public sealed record VerifyPhoneCommand(
    string CountryCode,
    string NationalNumber,
    string VerificationCode
);