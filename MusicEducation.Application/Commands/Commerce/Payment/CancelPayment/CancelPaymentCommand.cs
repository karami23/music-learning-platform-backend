namespace MusicEducation.Application.Commands.Commerce.Payment;

public sealed record CancelPaymentCommand(
    int UserId,
    int PaymentId);