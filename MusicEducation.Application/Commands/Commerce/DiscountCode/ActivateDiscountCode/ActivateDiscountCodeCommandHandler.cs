using MusicEducation.Application.Exceptions;
using MusicEducation.Application.Interfaces;
using MusicEducation.Domain.Interfaces.Commerce;

namespace MusicEducation.Application.Commands.Commerce.DiscountCode;

public sealed class ActivateDiscountCodeCommandHandler
{
    private readonly IDiscountCodeRepository _discountCodeRepository;
    private readonly IUnitOfWork _unitOfWork;

    public ActivateDiscountCodeCommandHandler(
        IDiscountCodeRepository discountCodeRepository,
        IUnitOfWork unitOfWork)
    {
        _discountCodeRepository = discountCodeRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task Handle(
        ActivateDiscountCodeCommand command,
        CancellationToken cancellationToken = default)
    {
        if (command.DiscountCodeId <= 0)
            throw new ValidationException(
                "شناسه کد تخفیف معتبر نیست");

        var discountCode = await _discountCodeRepository
            .GetByIdAsync(command.DiscountCodeId);

        if (discountCode is null)
            throw new NotFoundException(
                "کد تخفیف موردنظر پیدا نشد");

        discountCode.Activate();

        await _discountCodeRepository.UpdateAsync(discountCode);

        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }
}