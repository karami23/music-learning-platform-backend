namespace MusicEducation.Application.Commands.Authentication.LoginType;

public sealed record LoginCommand(
    LoginType LoginType,
    string? Email,
    string? CountryCode,
    string? NationalNumber,
    string Password
);