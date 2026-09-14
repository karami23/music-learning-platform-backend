using MusicEducation.Application.DTOs.Identity;
using MusicEducation.Application.Exceptions;
using MusicEducation.Application.Mappings;
using MusicEducation.Domain.Interfaces.Identity;

namespace MusicEducation.Application.Queries.Identity.User.GetUserById;

public sealed class GetUserByIdHandler
{
    private readonly IUserRepository _userRepository;

    public GetUserByIdHandler(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task<UserDto> Handle(GetUserByIdQuery query)
    {
        var user = await _userRepository.GetByIdAsync(query.UserId);

        if (user is null)
        {
            throw new NotFoundException("کاربر پیدا نشد");
        }

        return user.ToDto();
    }
}