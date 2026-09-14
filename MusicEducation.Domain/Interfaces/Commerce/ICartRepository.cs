using MusicEducation.Domain.Entities.Commerce;

namespace MusicEducation.Domain.Interfaces.Commerce;

public interface ICartRepository
{
    Task<Cart?> GetByIdAsync(int id);

    Task<Cart?> GetByUserIdAsync(int userId);

    Task<Cart> AddAsync(Cart cart);

    Task UpdateAsync(Cart cart);
}