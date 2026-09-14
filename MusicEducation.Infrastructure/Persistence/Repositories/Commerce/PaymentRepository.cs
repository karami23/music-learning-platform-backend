using Microsoft.EntityFrameworkCore;
using MusicEducation.Domain.Entities.Commerce;
using MusicEducation.Domain.Interfaces.Commerce;
using MusicEducation.Infrastructure.Persistence.DbContext;

namespace MusicEducation.Infrastructure.Repositories.Commerce;

public class PaymentRepository : IPaymentRepository
{
    private readonly MusicEducationDbContext _context;

    public PaymentRepository(MusicEducationDbContext context)
    {
        _context = context;
    }

    public async Task<Payment?> GetByIdAsync(int id)
    {
        return await _context.Set<Payment>()
            .FirstOrDefaultAsync(x => x.Id == id);
    }

    public async Task<Payment?> GetByOrderIdAsync(int orderId)
    {
        return await _context.Set<Payment>()
            .FirstOrDefaultAsync(x => x.OrderId == orderId);
    }

    public async Task<Payment?> GetByTransactionIdAsync(
        string transactionId)
    {
        if (string.IsNullOrWhiteSpace(transactionId))
            return null;

        return await _context.Set<Payment>()
            .FirstOrDefaultAsync(x =>
                x.TransactionId == transactionId);
    }

    public async Task<IEnumerable<Payment>> GetByUserIdAsync(
        int userId)
    {
        return await _context.Set<Payment>()
            .Where(payment =>
                _context.Set<Order>()
                    .Any(order =>
                        order.Id == payment.OrderId &&
                        order.UserId == userId))
            .ToListAsync();
    }

    public async Task<bool> ExistsByTransactionIdAsync(
        string transactionId)
    {
        if (string.IsNullOrWhiteSpace(transactionId))
            return false;

        return await _context.Set<Payment>()
            .AnyAsync(x => x.TransactionId == transactionId);
    }

    public async Task<Payment> AddAsync(Payment payment)
    {
        ArgumentNullException.ThrowIfNull(payment);

        await _context.Set<Payment>()
            .AddAsync(payment);

        return payment;
    }

    public Task UpdateAsync(Payment payment)
    {
        ArgumentNullException.ThrowIfNull(payment);

        _context.Set<Payment>()
            .Update(payment);

        return Task.CompletedTask;
    }
}