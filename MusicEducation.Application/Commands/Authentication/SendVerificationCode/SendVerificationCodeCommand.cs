namespace MusicEducation.Application.Commands.Authentication.SendVerificationCode;

public sealed record SendVerificationCodeCommand(
    string CountryCode,
    string NationalNumber
);