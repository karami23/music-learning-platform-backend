using MusicEducation.Application.Commands.Authentication.LoginType;
namespace MusicEducation.API.DTOs.Authentication;

public sealed record LoginRequest(
    LoginType LoginType,
    string? Email,
    string? CountryCode,
    string? NationalNumber,
    string Password
);
