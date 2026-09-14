using MusicEducation.Domain.Enums;

namespace MusicEducation.Application.DTOs.Commerce;

public record PaymentDto(
    int Id,
    int OrderId,
    decimal Amount,
    string Currency,
    PaymentStatus Status,
    string? TransactionId,
    string? ReferenceNumber,
    DateTime? PaidAt
);