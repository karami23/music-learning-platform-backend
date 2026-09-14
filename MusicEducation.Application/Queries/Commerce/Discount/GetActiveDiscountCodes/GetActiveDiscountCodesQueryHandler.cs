using MusicEducation.Application.DTOs.Commerce;
using MusicEducation.Application.Mappings.Commerce;
using MusicEducation.Domain.Interfaces.Commerce;

namespace MusicEducation.Application.Queries.Commerce.DiscountCode;

public class GetActiveDiscountCodesQueryHandler
{
    private readonly IDiscountCodeRepository _discountCodeRepository;

    public GetActiveDiscountCodesQueryHandler(IDiscountCodeRepository discountCodeRepository)
    {
        _discountCodeRepository = discountCodeRepository;
    }

    public async Task<IEnumerable<DiscountCodeDto>> Handle(GetActiveDiscountCodesQuery query)
    {
        var discountCodes = await _discountCodeRepository.GetActiveAsync();

        return discountCodes
            .Select(x => x.ToDto())
            .ToList();
    }
}