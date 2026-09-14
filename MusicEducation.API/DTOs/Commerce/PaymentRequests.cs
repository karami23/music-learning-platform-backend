namespace MusicEducation.API.DTOs.Commerce.Payment;

public sealed record CreatePaymentRequest(
    int OrderId);

public sealed record MarkPaymentAsPaidRequest(
    string TransactionId,
    string? ReferenceNumber);