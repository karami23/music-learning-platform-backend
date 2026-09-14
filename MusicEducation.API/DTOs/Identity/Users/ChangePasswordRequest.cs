namespace MusicEducation.API.DTOs.Users;

public sealed record ChangePasswordRequest(
    string CurrentPassword,
    string NewPassword
);