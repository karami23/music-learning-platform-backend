using MusicEducation.Domain.Constants;
using MusicEducation.Domain.Enums;
using MusicEducation.Domain.Exceptions;
using MusicEducation.Domain.ValueObjects;
using System.Text.RegularExpressions;

namespace MusicEducation.Domain.Entities.Commerce;

public class DiscountCode : BaseEntity
{
    private static readonly Regex CodeRegex = new(@"^[A-Z0-9]+(?:-[A-Z0-9]+)*$",
        RegexOptions.Compiled | RegexOptions.IgnoreCase);

    public string Code { get; private set; } = null!;

    public DiscountType Type { get; private set; }

    public decimal Value { get; private set; }

    public Money? MinimumOrderAmount { get; private set; }

    public int? UsageLimit { get; private set; }

    public int UsedCount { get; private set; }

    public DateRange ValidityPeriod { get; private set; } = null!;

    public bool IsActive { get; private set; }

    private DiscountCode() { }

    public static DiscountCode Create(
        string code,
        DiscountType type,
        decimal value,
        DateRange validityPeriod,
        Money? minimumOrderAmount = null,
        int? usageLimit = null)
    {
        var normalizedCode = NormalizeCode(code);

        ValidateType(type);
        ValidateValue(type, value);
        ValidateMinimumOrderAmount(minimumOrderAmount);
        ValidateUsageLimit(usageLimit);

        ArgumentNullException.ThrowIfNull(validityPeriod);

        return new DiscountCode
        {
            Code = normalizedCode,
            Type = type,
            Value = value,
            MinimumOrderAmount = minimumOrderAmount,
            UsageLimit = usageLimit,
            UsedCount = 0,
            ValidityPeriod = validityPeriod,
            IsActive = true
        };
    }

    public void Update(
        string code,
        DiscountType type,
        decimal value,
        DateRange validityPeriod,
        Money? minimumOrderAmount,
        int? usageLimit)
    {
        var normalizedCode = NormalizeCode(code);

        ValidateType(type);
        ValidateValue(type, value);
        ValidateMinimumOrderAmount(minimumOrderAmount);
        ValidateUsageLimit(usageLimit);

        ArgumentNullException.ThrowIfNull(validityPeriod);

        if (usageLimit.HasValue && usageLimit.Value < UsedCount)
            throw new DomainException(
                "سقف استفاده نمی‌تواند کمتر از تعداد استفاده فعلی باشد",
                nameof(usageLimit));

        Code = normalizedCode;
        Type = type;
        Value = value;
        ValidityPeriod = validityPeriod;
        MinimumOrderAmount = minimumOrderAmount;
        UsageLimit = usageLimit;

        UpdatedAt = DateTime.UtcNow;
    }

    public bool IsValid(DateTime utcNow)
    {
        if (!IsActive)
            return false;

        if (!ValidityPeriod.Contains(utcNow))
            return false;

        if (UsageLimit.HasValue && UsedCount >= UsageLimit.Value)
            return false;

        return true;
    }

    public Money CalculateDiscount(Money orderAmount)
    {
        ArgumentNullException.ThrowIfNull(orderAmount);

        if (MinimumOrderAmount is not null)
        {
            if (orderAmount.Currency != MinimumOrderAmount.Currency)
                throw new DomainException(
                    "واحد پول سفارش با حداقل مبلغ کد تخفیف یکسان نیست",
                    nameof(orderAmount));

            if (orderAmount.Amount < MinimumOrderAmount.Amount)
                throw new DomainException(
                    "مبلغ سفارش به حداقل مبلغ موردنیاز کد تخفیف نرسیده است",
                    nameof(orderAmount));
        }

        decimal discountAmount;

        if (Type == DiscountType.Percentage)
        {
            discountAmount = orderAmount.Amount * Value / 100;
        }
        else
        {
            discountAmount = Value;
        }

        discountAmount = Math.Min(
            discountAmount,
            orderAmount.Amount);

        return Money.Create(
            discountAmount,
            orderAmount.Currency);
    }

    public void RegisterUsage()
    {
        if (!IsActive)
            throw new DomainException("کد تخفیف فعال نیست", nameof(Code));

        if (UsageLimit.HasValue && UsedCount >= UsageLimit.Value)
            throw new DomainException("سقف استفاده از کد تخفیف تکمیل شده است", nameof(Code));

        UsedCount++;

        UpdatedAt = DateTime.UtcNow;
    }

    public void Activate()
    {
        IsActive = true;
        UpdatedAt = DateTime.UtcNow;
    }

    public void Deactivate()
    {
        IsActive = false;
        UpdatedAt = DateTime.UtcNow;
    }

    private static string NormalizeCode(string code)
    {
        if (string.IsNullOrWhiteSpace(code))
            throw new DomainException("کد تخفیف الزامی است", nameof(code));

        var normalized = code.Trim().ToUpperInvariant();

        if (normalized.Length < DomainConstants.MinDiscountCodeLength)
            throw new DomainException(
                $"کد تخفیف نمی‌تواند کمتر از {DomainConstants.MinDiscountCodeLength} کاراکتر باشد",
                nameof(code));

        if (normalized.Length > DomainConstants.MaxDiscountCodeLength)
            throw new DomainException(
                $"کد تخفیف نمی‌تواند بیشتر از {DomainConstants.MaxDiscountCodeLength} کاراکتر باشد",
                nameof(code));

        if (!CodeRegex.IsMatch(normalized))
            throw new DomainException("فرمت کد تخفیف معتبر نیست", nameof(code));

        return normalized;
    }

    private static void ValidateType(DiscountType type)
    {
        if (!Enum.IsDefined(type))
            throw new DomainException("نوع تخفیف معتبر نیست", nameof(type));
    }

    private static void ValidateValue(DiscountType type, decimal value)
    {
        if (value <= 0)
            throw new DomainException("مقدار تخفیف باید بزرگ‌تر از صفر باشد", nameof(value));

        if (type == DiscountType.Percentage && value > 100)
            throw new DomainException("درصد تخفیف نمی‌تواند بیشتر از 100 باشد", nameof(value));
    }

    private static void ValidateMinimumOrderAmount(
        Money? minimumOrderAmount)
    {
        if (minimumOrderAmount is not null && minimumOrderAmount.Amount <= 0)
        {
            throw new DomainException("حداقل مبلغ سفارش باید بزرگ‌تر از صفر باشد", nameof(minimumOrderAmount));
        }
    }

    private static void ValidateUsageLimit(int? usageLimit)
    {
        if (usageLimit.HasValue && usageLimit.Value <= 0)
            throw new DomainException("سقف استفاده باید بزرگ‌تر از صفر باشد", nameof(usageLimit));
    }
}