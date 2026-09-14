namespace MusicEducation.Application.Commands.Identity.Teacher.UpdateProfile;

public sealed record UpdateTeacherProfileCommand(
    int UserId,
    string? Bio
);