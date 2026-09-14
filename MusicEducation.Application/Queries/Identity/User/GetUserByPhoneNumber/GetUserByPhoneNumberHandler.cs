using MusicEducation.Application.DTOs.Identity;
using MusicEducation.Application.Exceptions;
using MusicEducation.Application.Mappings;
using MusicEducation.Domain.Interfaces.Identity;
using MusicEducation.Domain.ValueObjects;

namespace MusicEducation.Application.Queries.Identity.User.GetUserByPhoneNumber;

public sealed class GetUserByPhoneNumberHandler
{
    private readonly IUserRepository _userRepository;

    public GetUserByPhoneNumberHandler(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task<UserDto> Handle(GetUserByPhoneNumberQuery query)
    {
        var phoneNumber = PhoneNumber.Create(
            query.CountryCode,
            query.NationalNumber);

        var user = await _userRepository.GetByPhoneNumberAsync(phoneNumber);

        if (user is null)
        {
            throw new NotFoundException("کاربری با این شماره تلفن پیدا نشد");
        }

        return user.ToDto();
    }
}