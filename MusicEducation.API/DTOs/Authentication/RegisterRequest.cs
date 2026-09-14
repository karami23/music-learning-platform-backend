namespace MusicEducation.API.DTOs.Authentication;

public sealed record RegisterRequest(
    string Email,
    string Password,
    string FirstName,
    string LastName,
    string? CountryCode,
    string? NationalNumber
);