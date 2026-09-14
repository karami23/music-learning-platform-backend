namespace MusicEducation.Application.Commands.Authentication.ForgotPassword;

public sealed record ForgotPasswordCommand(
    string CountryCode,
    string NationalNumber
);