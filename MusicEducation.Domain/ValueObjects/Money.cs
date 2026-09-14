using MusicEducation.Domain.Exceptions;

namespace MusicEducation.Domain.ValueObjects;

public sealed record class Money
{
    public decimal Amount { get; }

    public string Currency { get; }

    private Money(
        decimal amount,
        string currency)
    {
        Amount = Math.Round(amount, 2);
        Currency = currency;
    }

    public static Money Create(
        decimal amount,
        string currency = "IRR")
    {
        if (amount < 0)
            throw new DomainException("مبلغ نمی‌تواند منفی باشد", nameof(amount));

        if (string.IsNullOrWhiteSpace(currency))
            throw new DomainException("واحد پول نمی‌تواند خالی باشد", nameof(currency));

        currency = currency.Trim().ToUpper();

        return new Money(amount, currency);
    }

    public override string ToString()
    {
        return $"{Amount:N0} {Currency}";
    }
}