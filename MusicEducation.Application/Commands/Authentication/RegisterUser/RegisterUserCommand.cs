using MusicEducation.Domain.Enums;

namespace MusicEducation.Application.Commands.Authentication.Register;

public sealed record RegisterUserCommand(
    string Email,
    string Password,
    string FirstName,
    string LastName,
    string? CountryCode,
    string? NationalNumber
);