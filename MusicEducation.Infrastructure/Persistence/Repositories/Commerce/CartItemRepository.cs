using Microsoft.EntityFrameworkCore;
using MusicEducation.Domain.Entities.Commerce;
using MusicEducation.Domain.Interfaces.Commerce;
using MusicEducation.Infrastructure.Persistence.DbContext;

namespace MusicEducation.Infrastructure.Repositories.Commerce;

public class CartItemRepository : ICartItemRepository
{
    private readonly MusicEducationDbContext _context;

    public CartItemRepository(MusicEducationDbContext context)
    {
        _context = context;
    }

    public async Task<CartItem?> GetByIdAsync(int id)
    {
        return await _context.Set<CartItem>()
            .FirstOrDefaultAsync(x => x.Id == id);
    }

    public async Task<CartItem?> GetByCartIdAndCourseIdAsync(
        int cartId,
        int courseId)
    {
        return await _context.Set<CartItem>()
            .FirstOrDefaultAsync(x =>
                x.CartId == cartId &&
                x.CourseId == courseId);
    }

    public async Task<IEnumerable<CartItem>> GetByCartIdAsync(
        int cartId)
    {
        return await _context.Set<CartItem>()
            .Where(x => x.CartId == cartId)
            .ToListAsync();
    }

    public async Task<bool> ExistsAsync(
        int cartId,
        int courseId)
    {
        return await _context.Set<CartItem>()
            .AnyAsync(x =>
                x.CartId == cartId &&
                x.CourseId == courseId);
    }

    public async Task<CartItem> AddAsync(CartItem cartItem)
    {
        ArgumentNullException.ThrowIfNull(cartItem);

        await _context.Set<CartItem>()
            .AddAsync(cartItem);

        return cartItem;
    }

    public Task UpdateAsync(CartItem cartItem)
    {
        ArgumentNullException.ThrowIfNull(cartItem);

        _context.Set<CartItem>()
            .Update(cartItem);

        return Task.CompletedTask;
    }

    public Task RemoveAsync(CartItem cartItem)
    {
        ArgumentNullException.ThrowIfNull(cartItem);

        _context.Set<CartItem>()
            .Remove(cartItem);

        return Task.CompletedTask;
    }
}