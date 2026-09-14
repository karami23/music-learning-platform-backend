using MusicEducation.Domain.Entities.Commerce;

namespace MusicEducation.Domain.Interfaces.Commerce;

public interface IPaymentRepository
{
    Task<Payment?> GetByIdAsync(int id);

    Task<Payment?> GetByOrderIdAsync(int orderId);

    Task<Payment?> GetByTransactionIdAsync(string transactionId);

    Task<IEnumerable<Payment>> GetByUserIdAsync(int userId);

    Task<bool> ExistsByTransactionIdAsync(string transactionId);

    Task<Payment> AddAsync(Payment payment);

    Task UpdateAsync(Payment payment);
}