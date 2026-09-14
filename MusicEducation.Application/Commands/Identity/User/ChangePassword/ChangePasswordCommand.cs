namespace MusicEducation.Application.Commands.Identity.User.ChangePassword;

public sealed record ChangePasswordCommand(
    int UserId,
    string CurrentPassword,
    string NewPassword
);