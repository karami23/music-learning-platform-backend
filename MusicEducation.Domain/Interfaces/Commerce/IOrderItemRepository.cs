using MusicEducation.Domain.Entities.Commerce;

namespace MusicEducation.Domain.Interfaces.Commerce;

public interface IOrderItemRepository
{
    Task<OrderItem?> GetByIdAsync(int id);

    Task<IEnumerable<OrderItem>> GetByOrderIdAsync(int orderId);

    Task<OrderItem?> GetByOrderIdAndCourseIdAsync(int orderId, int courseId);

    Task<bool> ExistsAsync(int orderId, int courseId);

    Task<OrderItem> AddAsync(OrderItem orderItem);

    Task UpdateAsync(OrderItem orderItem);
}