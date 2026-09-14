namespace MusicEducation.API.DTOs.Authentication;

public sealed record PhoneRequest(
    string CountryCode,
    string NationalNumber
);