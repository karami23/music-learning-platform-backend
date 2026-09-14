using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MusicEducation.API.DTOs.Articles.Article;
using MusicEducation.Application.Commands.Articles.Article;
using MusicEducation.Application.Queries.Articles.Article;

namespace MusicEducation.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public sealed class ArticleController : ControllerBase
{
    private readonly ArchiveArticleCommandHandler _archiveHandler;
    private readonly CreateArticleCommandHandler _createHandler;
    private readonly PublishArticleCommandHandler _publishHandler;
    private readonly UnpublishArticleCommandHandler _unpublishHandler;
    private readonly UpdateArticleCommandHandler _updateHandler;

    private readonly GetAllArticlesQueryHandler _getAllHandler;
    private readonly GetArticleByIdQueryHandler _getByIdHandler;
    private readonly GetArticleBySlugQueryHandler _getBySlugHandler;
    private readonly GetArticlesByCategoryIdQueryHandler _getByCategoryIdHandler;
    private readonly GetPublishedArticlesQueryHandler _getPublishedHandler;

    public ArticleController(
        ArchiveArticleCommandHandler archiveHandler,
        CreateArticleCommandHandler createHandler,
        PublishArticleCommandHandler publishHandler,
        UnpublishArticleCommandHandler unpublishHandler,
        UpdateArticleCommandHandler updateHandler,
        GetAllArticlesQueryHandler getAllHandler,
        GetArticleByIdQueryHandler getByIdHandler,
        GetArticleBySlugQueryHandler getBySlugHandler,
        GetArticlesByCategoryIdQueryHandler getByCategoryIdHandler,
        GetPublishedArticlesQueryHandler getPublishedHandler)
    {
        _archiveHandler = archiveHandler;
        _createHandler = createHandler;
        _publishHandler = publishHandler;
        _unpublishHandler = unpublishHandler;
        _updateHandler = updateHandler;

        _getAllHandler = getAllHandler;
        _getByIdHandler = getByIdHandler;
        _getBySlugHandler = getBySlugHandler;
        _getByCategoryIdHandler = getByCategoryIdHandler;
        _getPublishedHandler = getPublishedHandler;
    }

    [HttpPost]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Create(
        [FromBody] CreateArticleRequest request,
        CancellationToken cancellationToken)
    {
        var command = new CreateArticleCommand(
            request.ArticleCategoryId,
            request.Title,
            request.Slug,
            request.Summary,
            request.Content,
            request.CoverImageUrl,
            request.MetaTitle,
            request.MetaDescription);

        var articleId = await _createHandler
            .Handle(command, cancellationToken);

        return CreatedAtAction(
            nameof(GetById),
            new { articleId },
            new { articleId });
    }

    [HttpPut("{articleId:int}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Update(
        int articleId,
        [FromBody] UpdateArticleRequest request,
        CancellationToken cancellationToken)
    {
        var command = new UpdateArticleCommand(
            articleId,
            request.ArticleCategoryId,
            request.Title,
            request.Slug,
            request.Summary,
            request.Content,
            request.CoverImageUrl,
            request.MetaTitle,
            request.MetaDescription);

        await _updateHandler
            .Handle(command, cancellationToken);

        return NoContent();
    }

    [HttpPut("{articleId:int}/publish")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Publish(
        int articleId,
        CancellationToken cancellationToken)
    {
        var command = new PublishArticleCommand(articleId);

        await _publishHandler
            .Handle(command, cancellationToken);

        return NoContent();
    }

    [HttpPut("{articleId:int}/unpublish")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Unpublish(
        int articleId,
        CancellationToken cancellationToken)
    {
        var command = new UnpublishArticleCommand(articleId);

        await _unpublishHandler
            .Handle(command, cancellationToken);

        return NoContent();
    }

    [HttpPut("{articleId:int}/archive")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Archive(
        int articleId,
        CancellationToken cancellationToken)
    {
        var command = new ArchiveArticleCommand(articleId);

        await _archiveHandler
            .Handle(command, cancellationToken);

        return NoContent();
    }

    [HttpGet]
    [AllowAnonymous]
    public async Task<IActionResult> GetAll()
    {
        var query = new GetAllArticlesQuery();

        var result = await _getAllHandler
            .Handle(query);

        return Ok(result);
    }

    [HttpGet("published")]
    [AllowAnonymous]
    public async Task<IActionResult> GetPublished()
    {
        var query = new GetPublishedArticlesQuery();

        var result = await _getPublishedHandler
            .Handle(query);

        return Ok(result);
    }

    [HttpGet("{articleId:int}")]
    [AllowAnonymous]
    public async Task<IActionResult> GetById(
        int articleId)
    {
        var query = new GetArticleByIdQuery(articleId);

        var result = await _getByIdHandler
            .Handle(query);

        return Ok(result);
    }

    [HttpGet("slug/{slug}")]
    [AllowAnonymous]
    public async Task<IActionResult> GetBySlug(
        string slug)
    {
        var query = new GetArticleBySlugQuery(slug);

        var result = await _getBySlugHandler
            .Handle(query);

        return Ok(result);
    }

    [HttpGet("category/{categoryId:int}")]
    [AllowAnonymous]
    public async Task<IActionResult> GetByCategory(
        int categoryId)
    {
        var query = new GetArticlesByCategoryIdQuery(categoryId);

        var result = await _getByCategoryIdHandler
            .Handle(query);

        return Ok(result);
    }
}