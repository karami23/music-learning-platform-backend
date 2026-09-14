using MusicEducation.Domain.Entities.Identity;
using MusicEducation.Domain.Enums;
using MusicEducation.Domain.ValueObjects;

namespace MusicEducation.Domain.Interfaces.Identity;

public interface IUserRepository
{
    Task<User?> GetByIdAsync(int id);

    Task<User?> GetByEmailAsync(Email email);

    Task<User?> GetByPhoneNumberAsync(PhoneNumber phoneNumber);

    Task<IEnumerable<User>> GetAllAsync();

    Task<IEnumerable<User>> GetActiveUsersAsync();

    Task<IEnumerable<User>> GetByRoleAsync(UserRole role);

    Task<User> AddAsync(User user);

    Task UpdateAsync(User user);

    Task<bool> ExistsByEmailAsync(Email email);

    Task<bool> ExistsByPhoneNumberAsync(PhoneNumber phoneNumber);
}