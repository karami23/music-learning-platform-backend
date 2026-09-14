using ArticleEntity = MusicEducation.Domain.Entities.Articles.Article;
using MusicEducation.Application.Exceptions;
using MusicEducation.Application.Interfaces;
using MusicEducation.Domain.Interfaces.Articles;

namespace MusicEducation.Application.Commands.Articles.Article;

public sealed class CreateArticleCommandHandler
{
    private readonly IArticleRepository _articleRepository;
    private readonly IArticleCategoryRepository _categoryRepository;
    private readonly IUnitOfWork _unitOfWork;

    public CreateArticleCommandHandler(
        IArticleRepository articleRepository,
        IArticleCategoryRepository categoryRepository,
        IUnitOfWork unitOfWork)
    {
        _articleRepository = articleRepository;
        _categoryRepository = categoryRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<int> Handle(
        CreateArticleCommand command,
        CancellationToken cancellationToken = default)
    {
        if (command.ArticleCategoryId <= 0)
            throw new ValidationException(
                "شناسه دسته‌بندی مقاله معتبر نیست");

        if (string.IsNullOrWhiteSpace(command.Slug))
            throw new ValidationException(
                "اسلاگ مقاله الزامی است");

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

        if (existingArticle is not null)
            throw new ConflictException(
                "مقاله‌ای با این اسلاگ قبلاً وجود دارد");

        var article = ArticleEntity.Create(
            command.ArticleCategoryId,
            command.Title,
            normalizedSlug,
            command.Summary,
            command.Content,
            command.CoverImageUrl,
            command.MetaTitle,
            command.MetaDescription);

        await _articleRepository.AddAsync(article);

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return article.Id;
    }
}