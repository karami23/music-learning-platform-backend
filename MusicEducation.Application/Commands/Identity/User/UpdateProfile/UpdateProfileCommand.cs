namespace MusicEducation.Application.Commands.Identity.User.UpdateProfile;

public sealed record UpdateProfileCommand(
    int UserId,
    string FirstName,
    string LastName,
    string? CountryCode,
    string? NationalNumber,
    string? ProfileImageUrl
);