using MusicEducation.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace MusicEducation.Application.DTOs.Identity
{
    public record UserDto(
        int Id,
        string Email,
        string? PhoneNumber,
        bool IsPhoneVerified,
        string FirstName,
        string LastName,
        string? ProfileImageUrl,
        UserRole Role,
        UserStatus Status
    );
}
