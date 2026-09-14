using ArticleCategoryEntity =
    MusicEducation.Domain.Entities.Articles.ArticleCategory;
using MusicEducation.Application.Exceptions;
using MusicEducation.Application.Interfaces;
using MusicEducation.Domain.Interfaces.Articles;

namespace MusicEducation.Application.Commands.Articles.ArticleCategory;

public sealed class CreateArticleCategoryCommandHandler
{
    private readonly IArticleCategoryRepository _categoryRepository;
    private readonly IUnitOfWork _unitOfWork;

    public CreateArticleCategoryCommandHandler(
        IArticleCategoryRepository categoryRepository,
        IUnitOfWork unitOfWork)
    {
        _categoryRepository = categoryRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<int> Handle(
        CreateArticleCategoryCommand command,
        CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(command.Name))
            throw new ValidationException(
                "نام دسته‌بندی مقاله الزامی است");

        var normalizedName = command.Name.Trim();

        var categories = await _categoryRepository
            .GetAllAsync();

        var exists = categories.Any(x =>
            string.Equals(
                x.Name,
                normalizedName,
                StringComparison.OrdinalIgnoreCase));

        if (exists)
            throw new ConflictException(
                "دسته‌بندی مقاله با این نام قبلاً وجود دارد");

        var category = ArticleCategoryEntity.Create(
            normalizedName,
            command.Description);

        await _categoryRepository.AddAsync(category);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return category.Id;
    }
}