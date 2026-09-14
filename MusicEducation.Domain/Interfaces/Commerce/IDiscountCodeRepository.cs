using MusicEducation.Domain.Entities.Commerce;

namespace MusicEducation.Domain.Interfaces.Commerce;

public interface IDiscountCodeRepository
{
    Task<DiscountCode?> GetByIdAsync(int id);

    Task<DiscountCode?> GetByCodeAsync(string code);

    Task<IEnumerable<DiscountCode>> GetAllAsync();

    Task<IEnumerable<DiscountCode>> GetActiveAsync();

    Task<bool> ExistsByCodeAsync(string code);

    Task<DiscountCode> AddAsync(DiscountCode discountCode);

    Task UpdateAsync(DiscountCode discountCode);
}