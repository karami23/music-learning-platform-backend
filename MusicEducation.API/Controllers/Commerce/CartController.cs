using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MusicEducation.Application.Commands.Carts.Cart.AddItemToCart;
using MusicEducation.Application.Commands.Carts.Cart.ClearCart;
using MusicEducation.Application.Commands.Carts.Cart.CreateCart;
using MusicEducation.Application.Commands.Carts.Cart.RemoveItemFromCart;
using MusicEducation.Application.Queries.Commerce.Cart;
using MusicEducation.API.DTOs.Commerce.Cart;

namespace MusicEducation.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public sealed class CartController : ControllerBase
{
    private readonly AddItemToCartCommandHandler _addItemToCartHandler;
    private readonly ClearCartCommandHandler _clearCartHandler;
    private readonly CreateCartCommandHandler _createCartHandler;
    private readonly RemoveItemFromCartCommandHandler _removeItemFromCartHandler;

    private readonly GetCartByIdQueryHandler _getCartByIdHandler;
    private readonly GetCartByUserIdQueryHandler _getCartByUserIdHandler;

    public CartController(
        AddItemToCartCommandHandler addItemToCartHandler,
        ClearCartCommandHandler clearCartHandler,
        CreateCartCommandHandler createCartHandler,
        RemoveItemFromCartCommandHandler removeItemFromCartHandler,
        GetCartByIdQueryHandler getCartByIdHandler,
        GetCartByUserIdQueryHandler getCartByUserIdHandler)
    {
        _addItemToCartHandler = addItemToCartHandler;
        _clearCartHandler = clearCartHandler;
        _createCartHandler = createCartHandler;
        _removeItemFromCartHandler = removeItemFromCartHandler;
        _getCartByIdHandler = getCartByIdHandler;
        _getCartByUserIdHandler = getCartByUserIdHandler;
    }

    [Authorize]
    [HttpGet("my")]
    public async Task<IActionResult> GetMyCart(
        CancellationToken cancellationToken)
    {
        var userId = GetUserId();

        var query = new GetCartByUserIdQuery(userId);

        var result = await _getCartByUserIdHandler
            .HandleAsync(query);

        return Ok(result);
    }

    [Authorize]
    [HttpPost]
    public async Task<IActionResult> CreateCart(
        CancellationToken cancellationToken)
    {
        var userId = GetUserId();

        var command = new CreateCartCommand(userId);

        var cartId = await _createCartHandler
            .HandleAsync(command, cancellationToken);

        return CreatedAtAction(
            nameof(GetMyCart),
            new { },
            new { cartId });
    }

    [Authorize]
    [HttpPost("items")]
    public async Task<IActionResult> AddItem(
        [FromBody] AddItemToCartRequest request,
        CancellationToken cancellationToken)
    {
        var userId = GetUserId();

        var command = new AddItemToCartCommand(
            userId,
            request.CourseId);

        var cartItemId = await _addItemToCartHandler
            .HandleAsync(command, cancellationToken);

        return Ok(new { cartItemId });
    }

    [Authorize]
    [HttpDelete("items")]
    public async Task<IActionResult> RemoveItem(
        [FromBody] RemoveItemFromCartRequest request,
        CancellationToken cancellationToken)
    {
        var userId = GetUserId();

        var command = new RemoveItemFromCartCommand(
            userId,
            request.CourseId);

        await _removeItemFromCartHandler
            .HandleAsync(command, cancellationToken);

        return NoContent();
    }

    [Authorize]
    [HttpDelete("items/all")]
    public async Task<IActionResult> ClearCart(
        CancellationToken cancellationToken)
    {
        var userId = GetUserId();

        var command = new ClearCartCommand(userId);

        await _clearCartHandler
            .HandleAsync(command, cancellationToken);

        return NoContent();
    }


    [Authorize(Roles = "Admin")]
    [HttpGet("{cartId:int}")]
    public async Task<IActionResult> GetCartById(
        int cartId,
        CancellationToken cancellationToken)
    {
        var query = new GetCartByIdQuery(cartId);

        var result = await _getCartByIdHandler
            .HandleAsync(query);

        return Ok(result);
    }
    private int GetUserId()
    {
        var userIdClaim = User.FindFirst(
            ClaimTypes.NameIdentifier);

        if (userIdClaim is null ||
            !int.TryParse(userIdClaim.Value, out var userId) ||
            userId <= 0)
        {
            throw new UnauthorizedAccessException(
                "شناسه کاربر در توکن معتبر نیست");
        }

        return userId;
    }
}