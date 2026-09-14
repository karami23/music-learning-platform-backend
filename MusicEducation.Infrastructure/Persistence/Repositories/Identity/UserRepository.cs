using Microsoft.EntityFrameworkCore;
using MusicEducation.Domain.Entities.Identity;
using MusicEducation.Domain.Enums;
using MusicEducation.Domain.Interfaces.Identity;
using MusicEducation.Domain.ValueObjects;
using MusicEducation.Infrastructure.Persistence.DbContext;

namespace MusicEducation.Infrastructure.Repositories.Identity;

public class UserRepository : IUserRepository
{
    private readonly MusicEducationDbContext _context;

    public UserRepository(MusicEducationDbContext context)
    {
        _context = context;
    }

    public async Task<User?> GetByIdAsync(int id)
    {
        return await _context.Set<User>()
            .FirstOrDefaultAsync(x => x.Id == id);
    }

    public async Task<User?> GetByEmailAsync(Email email)
    {
        ArgumentNullException.ThrowIfNull(email);

        return await _context.Set<User>()
            .FirstOrDefaultAsync(x => x.Email == email);
    }

    public async Task<User?> GetByPhoneNumberAsync(PhoneNumber phoneNumber)
    {
        ArgumentNullException.ThrowIfNull(phoneNumber);

        return await _context.Set<User>()
            .FirstOrDefaultAsync(x => x.PhoneNumber == phoneNumber);
    }

    public async Task<IEnumerable<User>> GetAllAsync()
    {
        return await _context.Set<User>()
            .ToListAsync();
    }

    public async Task<IEnumerable<User>> GetActiveUsersAsync()
    {
        return await _context.Set<User>()
            .Where(x => x.Status == UserStatus.Active)
            .ToListAsync();
    }

    public async Task<IEnumerable<User>> GetByRoleAsync(UserRole role)
    {
        return await _context.Set<User>()
            .Where(x => x.Role == role)
            .ToListAsync();
    }

    public async Task<User> AddAsync(User user)
    {
        ArgumentNullException.ThrowIfNull(user);

        await _context.Set<User>().AddAsync(user);

        return user;
    }

    public Task UpdateAsync(User user)
    {
        ArgumentNullException.ThrowIfNull(user);

        _context.Set<User>().Update(user);

        return Task.CompletedTask;
    }

    public async Task<bool> ExistsByEmailAsync(Email email)
    {
        ArgumentNullException.ThrowIfNull(email);

        return await _context.Set<User>()
            .AnyAsync(x => x.Email == email);
    }

    public async Task<bool> ExistsByPhoneNumberAsync(PhoneNumber phoneNumber)
    {
        ArgumentNullException.ThrowIfNull(phoneNumber);

        return await _context.Set<User>()
            .AnyAsync(x => x.PhoneNumber == phoneNumber);
    }
}