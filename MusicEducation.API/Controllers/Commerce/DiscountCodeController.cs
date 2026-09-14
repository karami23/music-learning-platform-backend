using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MusicEducation.Application.Commands.Commerce.DiscountCode;
using MusicEducation.Application.Queries.Commerce.DiscountCode;
using MusicEducation.API.DTOs.Commerce.DiscountCode;

namespace MusicEducation.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize(Roles = "Admin")]
public sealed class DiscountCodeController : ControllerBase
{
    private readonly ActivateDiscountCodeCommandHandler _activateHandler;
    private readonly CreateDiscountCodeCommandHandler _createHandler;
    private readonly DeactivateDiscountCodeCommandHandler _deactivateHandler;
    private readonly UpdateDiscountCodeCommandHandler _updateHandler;

    private readonly GetActiveDiscountCodesQueryHandler _getActiveHandler;
    private readonly GetAllDiscountCodesQueryHandler _getAllHandler;
    private readonly GetDiscountCodeByCodeQueryHandler _getByCodeHandler;
    private readonly GetDiscountCodeByIdQueryHandler _getByIdHandler;

    public DiscountCodeController(
        ActivateDiscountCodeCommandHandler activateHandler,
        CreateDiscountCodeCommandHandler createHandler,
        DeactivateDiscountCodeCommandHandler deactivateHandler,
        UpdateDiscountCodeCommandHandler updateHandler,
        GetActiveDiscountCodesQueryHandler getActiveHandler,
        GetAllDiscountCodesQueryHandler getAllHandler,
        GetDiscountCodeByCodeQueryHandler getByCodeHandler,
        GetDiscountCodeByIdQueryHandler getByIdHandler)
    {
        _activateHandler = activateHandler;
        _createHandler = createHandler;
        _deactivateHandler = deactivateHandler;
        _updateHandler = updateHandler;

        _getActiveHandler = getActiveHandler;
        _getAllHandler = getAllHandler;
        _getByCodeHandler = getByCodeHandler;
        _getByIdHandler = getByIdHandler;
    }

    [HttpPost]
    public async Task<IActionResult> Create(
        [FromBody] CreateDiscountCodeRequest request,
        CancellationToken cancellationToken)
    {
        var command = new CreateDiscountCodeCommand(
            request.Code,
            request.Type,
            request.Value,
            request.StartDate,
            request.EndDate,
            request.MinimumOrderAmount,
            request.MinimumOrderAmountCurrency,
            request.UsageLimit);

        var discountCodeId = await _createHandler
            .Handle(command, cancellationToken);

        return CreatedAtAction(
            nameof(GetById),
            new { discountCodeId },
            new { discountCodeId });
    }

    [HttpPut("{discountCodeId:int}")]
    public async Task<IActionResult> Update(
        int discountCodeId,
        [FromBody] UpdateDiscountCodeRequest request,
        CancellationToken cancellationToken)
    {
        var command = new UpdateDiscountCodeCommand(
            discountCodeId,
            request.Code,
            request.Type,
            request.Value,
            request.StartDate,
            request.EndDate,
            request.MinimumOrderAmount,
            request.MinimumOrderAmountCurrency,
            request.UsageLimit);

        await _updateHandler
            .Handle(command, cancellationToken);

        return NoContent();
    }

    [HttpPut("{discountCodeId:int}/activate")]
    public async Task<IActionResult> Activate(
        int discountCodeId,
        CancellationToken cancellationToken)
    {
        var command = new ActivateDiscountCodeCommand(
            discountCodeId);

        await _activateHandler
            .Handle(command, cancellationToken);

        return NoContent();
    }

    [HttpPut("{discountCodeId:int}/deactivate")]
    public async Task<IActionResult> Deactivate(
        int discountCodeId,
        CancellationToken cancellationToken)
    {
        var command = new DeactivateDiscountCodeCommand(
            discountCodeId);

        await _deactivateHandler
            .Handle(command, cancellationToken);

        return NoContent();
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var query = new GetAllDiscountCodesQuery();

        var result = await _getAllHandler
            .Handle(query);

        return Ok(result);
    }

    [HttpGet("active")]
    public async Task<IActionResult> GetActive()
    {
        var query = new GetActiveDiscountCodesQuery();

        var result = await _getActiveHandler
            .Handle(query);

        return Ok(result);
    }
    [HttpGet("{discountCodeId:int}")]
    public async Task<IActionResult> GetById(
        int discountCodeId)
    {
        var query = new GetDiscountCodeByIdQuery(
            discountCodeId);

        var result = await _getByIdHandler
            .Handle(query);

        return Ok(result);
    }

    [HttpGet("code/{code}")]
    public async Task<IActionResult> GetByCode(
        string code)
    {
        var query = new GetDiscountCodeByCodeQuery(code);

        var result = await _getByCodeHandler
            .Handle(query);

        return Ok(result);
    }
}