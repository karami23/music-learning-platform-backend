using MusicEducation.Domain.Constants;
using MusicEducation.Domain.Enums;
using MusicEducation.Domain.Exceptions;
using MusicEducation.Domain.ValueObjects;

namespace MusicEducation.Domain.Entities.Commerce;

public class Order : BaseEntity
{
    public int UserId { get; private set; }

    public string OrderNumber { get; private set; } = null!;

    public OrderStatus Status { get; private set; }

    public Money Subtotal { get; private set; } = null!;

    public Money DiscountAmount { get; private set; } = null!;

    public Money TotalAmount { get; private set; } = null!;

    public int? DiscountCodeId { get; private set; }

    private readonly List<OrderItem> _items = new();

    public IReadOnlyCollection<OrderItem> Items => _items.AsReadOnly();

    private Order() { }

    public static Order Create(
        int userId,
        string orderNumber)
    {
        ValidateUserId(userId);
        ValidateOrderNumber(orderNumber);

        return new Order
        {
            UserId = userId,
            OrderNumber = orderNumber.Trim(),
            Status = OrderStatus.Pending,
            Subtotal = Money.Create(0),
            DiscountAmount = Money.Create(0),
            TotalAmount = Money.Create(0)
        };
    }

    public void AddItem(OrderItem item)
    {
        ArgumentNullException.ThrowIfNull(item);

        if (Status != OrderStatus.Pending)
            throw new DomainException(
                "امکان اضافه کردن آیتم به سفارش نهایی‌شده وجود ندارد",
                nameof(Status));

        if (_items.Any(x => x.CourseId == item.CourseId))
            throw new DomainException(
                "این دوره قبلاً در سفارش وجود دارد",
                nameof(item));

        _items.Add(item);

        RecalculateAmounts();

        UpdatedAt = DateTime.UtcNow;
    }

    public void RemoveItem(int courseId)
    {
        if (Status != OrderStatus.Pending)
            throw new DomainException(
                "امکان حذف آیتم از سفارش نهایی‌شده وجود ندارد",
                nameof(Status));

        if (courseId <= 0)
            throw new DomainException(
                "شناسه دوره معتبر نیست",
                nameof(courseId));

        var item = _items.FirstOrDefault(x => x.CourseId == courseId);

        if (item is null)
            throw new DomainException(
                "این دوره در سفارش وجود ندارد",
                nameof(courseId));

        _items.Remove(item);

        RecalculateAmounts();

        UpdatedAt = DateTime.UtcNow;
    }

    public void ApplyDiscount(int discountCodeId, Money discountAmount)
    {
        if (Status != OrderStatus.Pending)
            throw new DomainException(
                "فقط سفارش در انتظار پرداخت می‌تواند تخفیف دریافت کند",
                nameof(Status));

        if (discountCodeId <= 0)
            throw new DomainException(
                "شناسه کد تخفیف معتبر نیست",
                nameof(discountCodeId));

        ArgumentNullException.ThrowIfNull(discountAmount);

        if (discountAmount.Currency != Subtotal.Currency)
            throw new DomainException(
                "واحد پول تخفیف با واحد پول سفارش یکسان نیست",
                nameof(discountAmount));

        if (discountAmount.Amount < 0)
            throw new DomainException(
                "مبلغ تخفیف نمی‌تواند منفی باشد",
                nameof(discountAmount));

        if (discountAmount.Amount > Subtotal.Amount)
            throw new DomainException(
                "مبلغ تخفیف نمی‌تواند بیشتر از مبلغ سفارش باشد",
                nameof(discountAmount));

        DiscountCodeId = discountCodeId;
        DiscountAmount = discountAmount;

        RecalculateTotal();

        UpdatedAt = DateTime.UtcNow;
    }

    public void MarkAsPaid()
    {
        if (Status != OrderStatus.Pending)
            throw new DomainException(
                "فقط سفارش در انتظار پرداخت می‌تواند پرداخت شود",
                nameof(Status));

        if (!_items.Any())
            throw new DomainException(
                "سفارش بدون آیتم قابل پرداخت نیست",
                nameof(Items));

        Status = OrderStatus.Paid;

        UpdatedAt = DateTime.UtcNow;
    }

    public void MarkAsFailed()
    {
        if (Status != OrderStatus.Pending)
            throw new DomainException(
                "فقط سفارش در انتظار پرداخت می‌تواند ناموفق شود",
                nameof(Status));

        Status = OrderStatus.Failed;

        UpdatedAt = DateTime.UtcNow;
    }

    public void Cancel()
    {
        if (Status == OrderStatus.Paid)
            throw new DomainException(
                "سفارش پرداخت‌شده قابل لغو نیست",
                nameof(Status));

        if (Status == OrderStatus.Cancelled)
            return;

        Status = OrderStatus.Cancelled;

        UpdatedAt = DateTime.UtcNow;
    }

    private void RecalculateAmounts()
    {
        var subtotal = _items.Sum(x => x.UnitPrice.Amount);

        Subtotal = Money.Create(
            subtotal,
            GetCurrency());

        if (DiscountAmount.Amount > Subtotal.Amount)
            DiscountAmount = Money.Create(
                0,
                Subtotal.Currency);

        RecalculateTotal();
    }

    private void RecalculateTotal()
    {
        var totalAmount = Subtotal.Amount - DiscountAmount.Amount;

        if (totalAmount < 0)
            throw new DomainException(
                "مبلغ نهایی سفارش نمی‌تواند منفی باشد",
                nameof(TotalAmount));

        TotalAmount = Money.Create(totalAmount, Subtotal.Currency);
    }

    private string GetCurrency()
    {
        return _items
            .Select(x => x.UnitPrice.Currency)
            .FirstOrDefault()
            ?? "IRR";
    }

    private static void ValidateUserId(int userId)
    {
        if (userId <= 0)
            throw new DomainException("شناسه کاربر معتبر نیست", nameof(userId));
    }

    private static void ValidateOrderNumber(string orderNumber)
    {
        if (string.IsNullOrWhiteSpace(orderNumber))
            throw new DomainException("شماره سفارش الزامی است", nameof(orderNumber));

        if (orderNumber.Trim().Length >
            DomainConstants.MaxOrderNumberLength)
            throw new DomainException(
                $"شماره سفارش نمی‌تواند بیشتر از {DomainConstants.MaxOrderNumberLength} کاراکتر باشد",
                nameof(orderNumber));
    }
}