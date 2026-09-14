using Microsoft.EntityFrameworkCore;
using MusicEducation.Domain.Entities.Commerce;
using MusicEducation.Domain.Interfaces.Commerce;
using MusicEducation.Infrastructure.Persistence.DbContext;

namespace MusicEducation.Infrastructure.Repositories.Commerce;

public class OrderItemRepository : IOrderItemRepository
{
    private readonly MusicEducationDbContext _context;

    public OrderItemRepository(MusicEducationDbContext context)
    {
        _context = context;
    }

    public async Task<OrderItem?> GetByIdAsync(int id)
    {
        return await _context.Set<OrderItem>()
            .FirstOrDefaultAsync(x => x.Id == id);
    }

    public async Task<IEnumerable<OrderItem>> GetByOrderIdAsync(
        int orderId)
    {
        return await _context.Set<OrderItem>()
            .Where(x => x.OrderId == orderId)
            .ToListAsync();
    }

    public async Task<OrderItem?> GetByOrderIdAndCourseIdAsync(
        int orderId,
        int courseId)
    {
        return await _context.Set<OrderItem>()
            .FirstOrDefaultAsync(x =>
                x.OrderId == orderId &&
                x.CourseId == courseId);
    }

    public async Task<bool> ExistsAsync(
        int orderId,
        int courseId)
    {
        return await _context.Set<OrderItem>()
            .AnyAsync(x =>
                x.OrderId == orderId &&
                x.CourseId == courseId);
    }

    public async Task<OrderItem> AddAsync(OrderItem orderItem)
    {
        ArgumentNullException.ThrowIfNull(orderItem);

        await _context.Set<OrderItem>()
            .AddAsync(orderItem);

        return orderItem;
    }

    public Task UpdateAsync(OrderItem orderItem)
    {
        ArgumentNullException.ThrowIfNull(orderItem);

        _context.Set<OrderItem>()
            .Update(orderItem);

        return Task.CompletedTask;
    }
}