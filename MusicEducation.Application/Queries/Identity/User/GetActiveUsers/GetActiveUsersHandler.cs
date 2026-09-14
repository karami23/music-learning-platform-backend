using MusicEducation.Application.DTOs.Identity;
using MusicEducation.Application.Mappings;
using MusicEducation.Domain.Interfaces.Identity;

namespace MusicEducation.Application.Queries.Identity.User.GetActiveUsers;

public sealed class GetActiveUsersHandler
{
    private readonly IUserRepository _userRepository;

    public GetActiveUsersHandler(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task<IEnumerable<UserDto>> Handle(GetActiveUsersQuery query)
    {
        var users = await _userRepository.GetActiveUsersAsync();

        return users.Select(user => user.ToDto());
    }
}