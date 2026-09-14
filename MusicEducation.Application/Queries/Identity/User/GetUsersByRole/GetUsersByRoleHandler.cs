using MusicEducation.Application.DTOs.Identity;
using MusicEducation.Application.Mappings;
using MusicEducation.Domain.Enums;
using MusicEducation.Domain.Interfaces.Identity;

namespace MusicEducation.Application.Queries.Identity.User.GetUsersByRole;

public sealed class GetUsersByRoleHandler
{
    private readonly IUserRepository _userRepository;

    public GetUsersByRoleHandler(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task<IEnumerable<UserDto>> Handle(GetUsersByRoleQuery query)
    {
        var users = await _userRepository.GetByRoleAsync(query.Role);

        return users.Select(user => user.ToDto());
    }
}