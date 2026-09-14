namespace MusicEducation.Application.Commands.Commerce.Payment;

public record MarkPaymentAsPaidCommand(
    int PaymentId,
    string TransactionId,
    string? ReferenceNumber = null
);