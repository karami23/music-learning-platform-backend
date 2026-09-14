using MusicEducation.Application.Exceptions;
using MusicEducation.Application.Interfaces;
using MusicEducation.Domain.Interfaces.Commerce;
using MusicEducation.Domain.ValueObjects;
using DiscountCodeEntity =
    MusicEducation.Domain.Entities.Commerce.DiscountCode;

namespace MusicEducation.Application.Commands.Commerce.DiscountCode;

public sealed class CreateDiscountCodeCommandHandler
{
    private readonly IDiscountCodeRepository _discountCodeRepository;
    private readonly IUnitOfWork _unitOfWork;

    public CreateDiscountCodeCommandHandler(
        IDiscountCodeRepository discountCodeRepository,
        IUnitOfWork unitOfWork)
    {
        _discountCodeRepository = discountCodeRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<int> Handle(
        CreateDiscountCodeCommand command,
        CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(command.Code))
            throw new ValidationException(
                "کد تخفیف الزامی است");

        var code = command.Code
            .Trim()
            .ToUpperInvariant();

        if (await _discountCodeRepository.ExistsByCodeAsync(code))
            throw new ConflictException(
                "این کد تخفیف قبلاً ثبت شده است");

        var validityPeriod = DateRange.Create(
            command.StartDate,
            command.EndDate);

        Money? minimumOrderAmount = null;

        if (command.MinimumOrderAmount.HasValue)
        {
            if (string.IsNullOrWhiteSpace(
                command.MinimumOrderAmountCurrency))
            {
                throw new ValidationException(
                    "واحد پول حداقل مبلغ سفارش الزامی است");
            }

            minimumOrderAmount = Money.Create(
                command.MinimumOrderAmount.Value,
                command.MinimumOrderAmountCurrency);
        }

        var discountCode = DiscountCodeEntity.Create(
            code,
            command.Type,
            command.Value,
            validityPeriod,
            minimumOrderAmount,
            command.UsageLimit);

        await _discountCodeRepository.AddAsync(discountCode);

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return discountCode.Id;
    }
}