using Microsoft.EntityFrameworkCore;
using MusicEducation.Domain.Entities.Commerce;
using MusicEducation.Domain.Interfaces.Commerce;
using MusicEducation.Infrastructure.Persistence.DbContext;

namespace MusicEducation.Infrastructure.Repositories.Commerce;

public class CartRepository : ICartRepository
{
    private readonly MusicEducationDbContext _context;

    public CartRepository(MusicEducationDbContext context)
    {
        _context = context;
    }

    public async Task<Cart?> GetByIdAsync(int id)
    {
        return await _context.Set<Cart>()
            .Include("_items")
            .FirstOrDefaultAsync(x => x.Id == id);
    }

    public async Task<Cart?> GetByUserIdAsync(int userId)
    {
        return await _context.Set<Cart>()
            .Include("_items")
            .FirstOrDefaultAsync(x => x.UserId == userId);
    }

    public async Task<Cart> AddAsync(Cart cart)
    {
        ArgumentNullException.ThrowIfNull(cart);

        await _context.Set<Cart>().AddAsync(cart);

        return cart;
    }

    public Task UpdateAsync(Cart cart)
    {
        ArgumentNullException.ThrowIfNull(cart);

        _context.Set<Cart>().Update(cart);

        return Task.CompletedTask;
    }
}