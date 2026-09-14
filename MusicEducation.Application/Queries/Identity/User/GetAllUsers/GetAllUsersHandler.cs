using MusicEducation.Application.DTOs.Identity;
using MusicEducation.Application.Mappings;
using MusicEducation.Domain.Interfaces.Identity;

namespace MusicEducation.Application.Queries.Identity.User.GetAllUsers;

public sealed class GetAllUsersHandler
{
    private readonly IUserRepository _userRepository;

    public GetAllUsersHandler(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task<IEnumerable<UserDto>> Handle(GetAllUsersQuery query)
    {
        var users = await _userRepository.GetAllAsync();

        return users.Select(user => user.ToDto());
    }
}