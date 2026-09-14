using MusicEducation.Application.Exceptions;
using MusicEducation.Application.Interfaces;
using MusicEducation.Domain.Interfaces.Articles;

namespace MusicEducation.Application.Commands.Articles.Article;

public sealed class UpdateArticleCommandHandler
{
    private readonly IArticleRepository _articleRepository;
    private readonly IArticleCategoryRepository _categoryRepository;
    private readonly IUnitOfWork _unitOfWork;

    public UpdateArticleCommandHandler(
        IArticleRepository articleRepository,
        IArticleCategoryRepository categoryRepository,
        IUnitOfWork unitOfWork)
    {
        _articleRepository = articleRepository;
        _categoryRepository = categoryRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task Handle(
        UpdateArticleCommand command,
        CancellationToken cancellationToken = default)
    {
        if (command.Id <= 0)
            throw new ValidationException("شناسه مقاله معتبر نیست");

        if (command.ArticleCategoryId <= 0)
            throw new ValidationException(
                "شناسه دسته‌بندی مقاله معتبر نیست");

        if (string.IsNullOrWhiteSpace(command.Slug))
            throw new ValidationException(
                "اسلاگ مقاله الزامی است");

        var article = await _articleRepository
            .GetByIdAsync(command.Id);

        if (article is null)
            throw new NotFoundException(
                "مقاله موردنظر پیدا نشد");

        var category = await _categoryRepository
            .GetByIdAsync(command.ArticleCategoryId);

        if (category is null)
            throw new NotFoundException(
                "دسته‌بندی مقاله موردنظر پیدا نشد");

        if (!category.IsActive)
            throw new ConflictException(
                "دسته‌بندی مقاله غیرفعال است");

        var normalizedSlug = command.Slug
            .Trim()
            .ToLowerInvariant();

        var existingArticle = await _articleRepository
            .GetBySlugAsync(normalizedSlug);

        if (existingArticle is not null &&
            existingArticle.Id != article.Id)
        {
            throw new ConflictException(
                "مقاله‌ای با این اسلاگ قبلاً وجود دارد");
        }

        article.Update(
            command.ArticleCategoryId,
            command.Title,
            normalizedSlug,
            command.Summary,
            command.Content,
            command.CoverImageUrl,
            command.MetaTitle,
            command.MetaDescription);

        await _articleRepository.UpdateAsync(article);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }
}