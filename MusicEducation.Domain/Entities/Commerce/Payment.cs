using MusicEducation.Domain.Enums;
using MusicEducation.Domain.Exceptions;
using MusicEducation.Domain.ValueObjects;

namespace MusicEducation.Domain.Entities.Commerce;

public class Payment : BaseEntity
{
    public int OrderId { get; private set; }

    public Money Amount { get; private set; } = null!;

    public PaymentStatus Status { get; private set; }

    public string? TransactionId { get; private set; }

    public string? ReferenceNumber { get; private set; }

    public DateTime? PaidAt { get; private set; }

    private Payment() { }

    public static Payment Create(int orderId, Money amount)
    {
        if (orderId <= 0)
            throw new DomainException("شناسه سفارش معتبر نیست", nameof(orderId));

        ArgumentNullException.ThrowIfNull(amount);

        if (amount.Amount <= 0)
            throw new DomainException("مبلغ پرداخت باید بزرگ‌تر از صفر باشد", nameof(amount));

        return new Payment
        {
            OrderId = orderId,
            Amount = amount,
            Status = PaymentStatus.Pending
        };
    }

    public void MarkAsPaid(string transactionId, string? referenceNumber = null)
    {
        if (Status != PaymentStatus.Pending)
            throw new DomainException("فقط پرداخت در انتظار می‌تواند موفق شود", nameof(Status));

        if (string.IsNullOrWhiteSpace(transactionId))
            throw new DomainException("شناسه تراکنش الزامی است", nameof(transactionId));

        TransactionId = transactionId.Trim();

        ReferenceNumber = string.IsNullOrWhiteSpace(referenceNumber) ? null : referenceNumber.Trim();

        Status = PaymentStatus.Paid;
        PaidAt = DateTime.UtcNow;
        UpdatedAt = DateTime.UtcNow;
    }

    public void MarkAsFailed()
    {
        if (Status != PaymentStatus.Pending)
            throw new DomainException("فقط پرداخت در انتظار می‌تواند ناموفق شود", nameof(Status));

        Status = PaymentStatus.Failed;
        UpdatedAt = DateTime.UtcNow;
    }

    public void Cancel()
    {
        if (Status == PaymentStatus.Paid)
            throw new DomainException("پرداخت موفق قابل لغو نیست", nameof(Status));

        if (Status == PaymentStatus.Cancelled)
            return;

        Status = PaymentStatus.Cancelled;
        UpdatedAt = DateTime.UtcNow;
    }
}