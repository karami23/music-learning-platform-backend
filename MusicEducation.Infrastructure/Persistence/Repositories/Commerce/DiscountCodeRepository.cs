using Microsoft.EntityFrameworkCore;
using MusicEducation.Domain.Entities.Commerce;
using MusicEducation.Domain.Interfaces.Commerce;
using MusicEducation.Infrastructure.Persistence.DbContext;

namespace MusicEducation.Infrastructure.Repositories.Commerce;

public class DiscountCodeRepository : IDiscountCodeRepository
{
    private readonly MusicEducationDbContext _context;

    public DiscountCodeRepository(MusicEducationDbContext context)
    {
        _context = context;
    }

    public async Task<DiscountCode?> GetByIdAsync(int id)
    {
        return await _context.Set<DiscountCode>()
            .FirstOrDefaultAsync(x => x.Id == id);
    }

    public async Task<DiscountCode?> GetByCodeAsync(string code)
    {
        if (string.IsNullOrWhiteSpace(code))
            return null;

        var normalizedCode = code.Trim().ToUpperInvariant();

        return await _context.Set<DiscountCode>()
            .FirstOrDefaultAsync(x => x.Code == normalizedCode);
    }

    public async Task<IEnumerable<DiscountCode>> GetAllAsync()
    {
        return await _context.Set<DiscountCode>()
            .ToListAsync();
    }

    public async Task<IEnumerable<DiscountCode>> GetActiveAsync()
    {
        return await _context.Set<DiscountCode>()
            .Where(x => x.IsActive)
            .ToListAsync();
    }

    public async Task<bool> ExistsByCodeAsync(string code)
    {
        if (string.IsNullOrWhiteSpace(code))
            return false;

        var normalizedCode = code.Trim().ToUpperInvariant();

        return await _context.Set<DiscountCode>()
            .AnyAsync(x => x.Code == normalizedCode);
    }

    public async Task<DiscountCode> AddAsync(
        DiscountCode discountCode)
    {
        ArgumentNullException.ThrowIfNull(discountCode);

        await _context.Set<DiscountCode>()
            .AddAsync(discountCode);

        return discountCode;
    }

    public Task UpdateAsync(DiscountCode discountCode)
    {
        ArgumentNullException.ThrowIfNull(discountCode);

        _context.Set<DiscountCode>()
            .Update(discountCode);

        return Task.CompletedTask;
    }
}