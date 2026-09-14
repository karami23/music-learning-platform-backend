namespace MusicEducation.Application.Interfaces;

public record PaymentVerificationResult(
    bool IsSuccessful,
    string? ReferenceNumber,
    string? Message);