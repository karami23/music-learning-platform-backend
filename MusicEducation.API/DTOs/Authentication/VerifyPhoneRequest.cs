namespace MusicEducation.API.DTOs.Authentication;

public sealed record VerifyPhoneRequest(
    string CountryCode,
    string NationalNumber,
    string VerificationCode
);