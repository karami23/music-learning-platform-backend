using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MusicEducation.API.DTOs.Articles.ArticleCategory;
using MusicEducation.Application.Commands.Articles.ArticleCategory;
using MusicEducation.Application.Exceptions;
using MusicEducation.Application.Queries.Articles.ArticleCategory;

namespace MusicEducation.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public sealed class ArticleCategoryController : ControllerBase
{
    private readonly ActivateArticleCategoryCommandHandler _activateHandler;
    private readonly CreateArticleCategoryCommandHandler _createHandler;
    private readonly DeactivateArticleCategoryCommandHandler _deactivateHandler;
    private readonly UpdateArticleCategoryCommandHandler _updateHandler;

    private readonly GetActiveArticleCategoriesQueryHandler _getActiveHandler;
    private readonly GetAllArticleCategoriesQueryHandler _getAllHandler;
    private readonly GetArticleCategoryByIdQueryHandler _getByIdHandler;

    public ArticleCategoryController(
        ActivateArticleCategoryCommandHandler activateHandler,
        CreateArticleCategoryCommandHandler createHandler,
        DeactivateArticleCategoryCommandHandler deactivateHandler,
        UpdateArticleCategoryCommandHandler updateHandler,
        GetActiveArticleCategoriesQueryHandler getActiveHandler,
        GetAllArticleCategoriesQueryHandler getAllHandler,
        GetArticleCategoryByIdQueryHandler getByIdHandler)
    {
        _activateHandler = activateHandler;
        _createHandler = createHandler;
        _deactivateHandler = deactivateHandler;
        _updateHandler = updateHandler;

        _getActiveHandler = getActiveHandler;
        _getAllHandler = getAllHandler;
        _getByIdHandler = getByIdHandler;
    }

    [HttpPost]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Create(
        [FromBody] CreateArticleCategoryRequest request,
        CancellationToken cancellationToken)
    {
        var command = new CreateArticleCategoryCommand(
            request.Name,
            request.Description);

        var categoryId = await _createHandler
            .Handle(command, cancellationToken);

        return CreatedAtAction(
            nameof(GetById),
            new { categoryId },
            new { categoryId });
    }

    [HttpPut("{categoryId:int}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Update(
        int categoryId,
        [FromBody] UpdateArticleCategoryRequest request,
        CancellationToken cancellationToken)
    {
        var command = new UpdateArticleCategoryCommand(
            categoryId,
            request.Name,
            request.Description);

        await _updateHandler
            .Handle(command, cancellationToken);

        return NoContent();
    }

    [HttpPut("{categoryId:int}/activate")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Activate(
        int categoryId,
        CancellationToken cancellationToken)
    {
        var command = new ActivateArticleCategoryCommand(
            categoryId);

        await _activateHandler
            .Handle(command, cancellationToken);

        return NoContent();
    }

    [HttpPut("{categoryId:int}/deactivate")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Deactivate(
        int categoryId,
        CancellationToken cancellationToken)
    {
        var command = new DeactivateArticleCategoryCommand(
            categoryId);

        await _deactivateHandler
            .Handle(command, cancellationToken);

        return NoContent();
    }

    [HttpGet]
    [AllowAnonymous]
    public async Task<IActionResult> GetActive()
    {
        var query = new GetActiveArticleCategoriesQuery();

        var result = await _getActiveHandler
            .Handle(query);

        return Ok(result);
    }

    [HttpGet("all")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> GetAll()
    {
        var query = new GetAllArticleCategoriesQuery();

        var result = await _getAllHandler
            .Handle(query);

        return Ok(result);
    }

    [HttpGet("{categoryId:int}")]
    [AllowAnonymous]
    public async Task<IActionResult> GetById(
        int categoryId)
    {
        var query = new GetArticleCategoryByIdQuery(
            categoryId);

        var result = await _getByIdHandler
            .Handle(query);

        return Ok(result);
    }
}