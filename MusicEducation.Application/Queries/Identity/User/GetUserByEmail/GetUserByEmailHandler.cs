using MusicEducation.Application.DTOs.Identity;
using MusicEducation.Application.Exceptions;
using MusicEducation.Application.Mappings;
using MusicEducation.Domain.Interfaces.Identity;
using MusicEducation.Domain.ValueObjects;

namespace MusicEducation.Application.Queries.Identity.User.GetUserByEmail;

public sealed class GetUserByEmailHandler
{
    private readonly IUserRepository _userRepository;

    public GetUserByEmailHandler(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task<UserDto> Handle(GetUserByEmailQuery query)
    {
        var email = Email.Create(query.Email);

        var user = await _userRepository.GetByEmailAsync(email);

        if (user is null)
        {
            throw new NotFoundException("کاربری با این ایمیل پیدا نشد");
        }

        return user.ToDto();
    }
}