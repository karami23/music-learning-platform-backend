namespace MusicEducation.Application.Commands.Commerce.Payment;

public sealed record CreatePaymentCommand(
    int UserId,
    int OrderId);