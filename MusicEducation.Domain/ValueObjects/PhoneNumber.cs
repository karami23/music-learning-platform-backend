using System.Text.RegularExpressions;
using MusicEducation.Domain.Exceptions;

namespace MusicEducation.Domain.ValueObjects;

public sealed record class PhoneNumber
{
    public string CountryCode { get; }

    public string NationalNumber { get; }

    public string Value => $"{CountryCode}{NationalNumber}";

    private static readonly Regex CountryCodeRegex = new(@"^\+[1-9]\d{0,2}$", RegexOptions.Compiled);

    private static readonly Regex NationalNumberRegex = new(@"^[1-9]\d{6,14}$", RegexOptions.Compiled);

    private PhoneNumber(
        string countryCode,
        string nationalNumber)
    {
        CountryCode = countryCode;
        NationalNumber = nationalNumber;
    }

    public static PhoneNumber Create(
        string countryCode,
        string nationalNumber)
    {
        countryCode = Normalize(countryCode);
        nationalNumber = Normalize(nationalNumber);

        ValidateCountryCode(countryCode);
        ValidateNationalNumber(nationalNumber);

        return new PhoneNumber(countryCode, nationalNumber);
    }

    private static string Normalize(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new DomainException("شماره تلفن نمی‌تواند خالی باشد", nameof(value));

        return value.Trim().Replace(" ", "");
    }

    private static void ValidateCountryCode(string countryCode)
    {
        if (!CountryCodeRegex.IsMatch(countryCode))
            throw new DomainException("کد کشور معتبر نیست", nameof(countryCode));
    }

    private static void ValidateNationalNumber(string nationalNumber)
    {
        if (!NationalNumberRegex.IsMatch(nationalNumber))
            throw new DomainException("شماره تلفن معتبر نیست", nameof(nationalNumber));
    }

    public override string ToString()
    {
        return Value;
    }
}