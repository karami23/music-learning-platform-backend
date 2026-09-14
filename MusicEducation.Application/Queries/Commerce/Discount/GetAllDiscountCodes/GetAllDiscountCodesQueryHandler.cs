using MusicEducation.Application.DTOs.Commerce;
using MusicEducation.Application.Mappings.Commerce;
using MusicEducation.Domain.Interfaces.Commerce;

namespace MusicEducation.Application.Queries.Commerce.DiscountCode;

public class GetAllDiscountCodesQueryHandler
{
    private readonly IDiscountCodeRepository _discountCodeRepository;

    public GetAllDiscountCodesQueryHandler(IDiscountCodeRepository discountCodeRepository)
    {
        _discountCodeRepository = discountCodeRepository;
    }

    public async Task<IEnumerable<DiscountCodeDto>> Handle(GetAllDiscountCodesQuery query)
    {
        var discountCodes = await _discountCodeRepository.GetAllAsync();

        return discountCodes
            .Select(x => x.ToDto())
            .ToList();
    }
}