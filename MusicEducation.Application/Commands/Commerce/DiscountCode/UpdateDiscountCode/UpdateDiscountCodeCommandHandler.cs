using MusicEducation.Application.Exceptions;
using MusicEducation.Application.Interfaces;
using MusicEducation.Domain.Interfaces.Commerce;
using MusicEducation.Domain.ValueObjects;

namespace MusicEducation.Application.Commands.Commerce.DiscountCode;

public sealed class UpdateDiscountCodeCommandHandler
{
    private readonly IDiscountCodeRepository _discountCodeRepository;
    private readonly IUnitOfWork _unitOfWork;

    public UpdateDiscountCodeCommandHandler(
        IDiscountCodeRepository discountCodeRepository,
        IUnitOfWork unitOfWork)
    {
        _discountCodeRepository = discountCodeRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task Handle(
        UpdateDiscountCodeCommand command,
        CancellationToken cancellationToken = default)
    {
        if (command.Id <= 0)
            throw new ValidationException(
                "شناسه کد تخفیف معتبر نیست");

        if (string.IsNullOrWhiteSpace(command.Code))
            throw new ValidationException(
                "کد تخفیف الزامی است");

        var discountCode = await _discountCodeRepository
            .GetByIdAsync(command.Id);

        if (discountCode is null)
            throw new NotFoundException(
                "کد تخفیف موردنظر پیدا نشد");

        var normalizedCode = command.Code
            .Trim()
            .ToUpperInvariant();

        var existingCode = await _discountCodeRepository
            .GetByCodeAsync(normalizedCode);

        if (existingCode is not null &&
            existingCode.Id != discountCode.Id)
        {
            throw new ConflictException(
                "این کد تخفیف قبلاً ثبت شده است");
        }

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

        discountCode.Update(
            normalizedCode,
            command.Type,
            command.Value,
            validityPeriod,
            minimumOrderAmount,
            command.UsageLimit);

        await _discountCodeRepository.UpdateAsync(discountCode);

        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }
}