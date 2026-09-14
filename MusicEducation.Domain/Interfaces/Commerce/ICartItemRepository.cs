using MusicEducation.Domain.Entities.Commerce;

namespace MusicEducation.Domain.Interfaces.Commerce;

public interface ICartItemRepository
{
    Task<CartItem?> GetByIdAsync(int id);

    Task<CartItem?> GetByCartIdAndCourseIdAsync(int cartId, int courseId);

    Task<IEnumerable<CartItem>> GetByCartIdAsync(int cartId);

    Task<bool> ExistsAsync(int cartId, int courseId);

    Task<CartItem> AddAsync(CartItem cartItem);

    Task UpdateAsync(CartItem cartItem);

    Task RemoveAsync(CartItem cartItem);
}