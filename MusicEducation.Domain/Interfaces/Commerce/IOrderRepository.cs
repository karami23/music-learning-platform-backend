using MusicEducation.Domain.Entities.Commerce;
using MusicEducation.Domain.Enums;

namespace MusicEducation.Domain.Interfaces.Commerce;

public interface IOrderRepository
{
    Task<Order?> GetByIdAsync(int id);

    Task<Order?> GetByOrderNumberAsync(string orderNumber);

    Task<IEnumerable<Order>> GetByUserIdAsync(int userId);

    Task<IEnumerable<Order>> GetByUserIdAndStatusAsync(int userId, OrderStatus status);

    Task<bool> ExistsByOrderNumberAsync(string orderNumber);

    Task<Order> AddAsync(Order order);

    Task UpdateAsync(Order order);
}