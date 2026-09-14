namespace MusicEducation.Application.Commands.Identity.User.PromoteUserToTeacher;

public sealed record PromoteUserToTeacherCommand(
    int UserId,
    string? Bio
);