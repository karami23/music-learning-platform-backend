using MusicEducation.Domain.Enums;

namespace MusicEducation.Application.Queries.Identity.User.GetUsersByRole;

public sealed record GetUsersByRoleQuery(UserRole Role);