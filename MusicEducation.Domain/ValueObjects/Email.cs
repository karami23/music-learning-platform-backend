using System.Text.RegularExpressions;
using MusicEducation.Domain.Exceptions;

namespace MusicEducation.Domain.ValueObjects;

public sealed record class Email
{
    public string Value { get; }

    private static readonly Regex EmailRegex = new(@"^[^@\s]+@[^@\s]+\.[^@\s]+$",
        RegexOptions.Compiled | RegexOptions.IgnoreCase);

    private Email(string value)
    {
        Value = value;
    }

    public static Email Create(string value)
    {
        ValidateRequired(value);

        value = value.Trim().ToLowerInvariant();

        ValidateFormat(value);

        return new Email(value);
    }

    private static void ValidateRequired(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new DomainException("ایمیل نمی‌تواند خالی باشد", nameof(value));
    }

    private static void ValidateFormat(string value)
    {
        if (!EmailRegex.IsMatch(value))
            throw new DomainException("فرمت ایمیل معتبر نیست", nameof(value));
    }

    public override string ToString()
    {
        return Value;
    }
}