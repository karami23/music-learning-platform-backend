using MusicEducation.Application.DTOs;
using MusicEducation.Application.DTOs.Identity;
using MusicEducation.Domain.Entities.Identity;

namespace MusicEducation.Application.Mappings;

public static class UserMapping
{
    public static UserDto ToDto(this User user)
    {
        return new UserDto(
            user.Id,
            user.Email.Value,
            user.PhoneNumber?.Value,
            user.IsPhoneVerified,
            user.FirstName,
            user.LastName,
            user.ProfileImageUrl,
            user.Role,
            user.Status
        );
    }
}