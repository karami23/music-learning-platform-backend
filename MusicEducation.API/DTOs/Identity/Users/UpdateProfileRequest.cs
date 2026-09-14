namespace MusicEducation.API.DTOs.Users;

public sealed record UpdateProfileRequest(
    string FirstName,
    string LastName,
    string? CountryCode,
    string? NationalNumber,
    string? ProfileImageUrl
);