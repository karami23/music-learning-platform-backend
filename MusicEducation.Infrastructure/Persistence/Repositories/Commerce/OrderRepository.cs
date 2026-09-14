using Microsoft.EntityFrameworkCore;
using MusicEducation.Domain.Entities.Commerce;
using MusicEducation.Domain.Enums;
using MusicEducation.Domain.Interfaces.Commerce;
using MusicEducation.Infrastructure.Persistence.DbContext;

namespace MusicEducation.Infrastructure.Repositories.Commerce;

public class OrderRepository : IOrderRepository
{
    private readonly MusicEducationDbContext _context;

    public OrderRepository(MusicEducationDbContext context)
    {
        _context = context;
    }

    public async Task<Order?> GetByIdAsync(int id)
    {
        return await _context.Set<Order>()
            .FirstOrDefaultAsync(x => x.Id == id);
    }

    public async Task<Order?> GetByOrderNumberAsync(
        string orderNumber)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(orderNumber);

        return await _context.Set<Order>()
            .FirstOrDefaultAsync(x =>
                x.OrderNumber == orderNumber);
    }

    public async Task<IEnumerable<Order>> GetByUserIdAsync(
        int userId)
    {
        if (userId <= 0)
            throw new ArgumentException(
                "شناسه کاربر معتبر نیست.",
                nameof(userId));

        return await _context.Set<Order>()
            .Where(x => x.UserId == userId)
            .OrderByDescending(x => x.CreatedAt)
            .ToListAsync();
    }

    public async Task<IEnumerable<Order>> GetByUserIdAndStatusAsync(
        int userId,
        OrderStatus status)
    {
        if (userId <= 0)
            throw new ArgumentException(
                "شناسه کاربر معتبر نیست.",
                nameof(userId));

        return await _context.Set<Order>()
            .Where(x =>
                x.UserId == userId &&
                x.Status == status)
            .OrderByDescending(x => x.CreatedAt)
            .ToListAsync();
    }

    public async Task<bool> ExistsByOrderNumberAsync(
        string orderNumber)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(orderNumber);

        return await _context.Set<Order>()
            .AnyAsync(x =>
                x.OrderNumber == orderNumber);
    }

    public async Task<Order> AddAsync(Order order)
    {
        ArgumentNullException.ThrowIfNull(order);

        await _context.Set<Order>()
            .AddAsync(order);

        return order;
    }

    public Task UpdateAsync(Order order)
    {
        ArgumentNullException.ThrowIfNull(order);

        _context.Set<Order>()
            .Update(order);

        return Task.CompletedTask;
    }
}