using MusicEducation.Application.Exceptions;
using MusicEducation.Application.Interfaces;
using MusicEducation.Domain.Interfaces.Articles;

namespace MusicEducation.Application.Commands.Articles.ArticleCategory;

public sealed class ActivateArticleCategoryCommandHandler
{
    private readonly IArticleCategoryRepository _categoryRepository;
    private readonly IUnitOfWork _unitOfWork;

    public ActivateArticleCategoryCommandHandler(
        IArticleCategoryRepository categoryRepository,
        IUnitOfWork unitOfWork)
    {
        _categoryRepository = categoryRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task Handle(
        ActivateArticleCategoryCommand command,
        CancellationToken cancellationToken = default)
    {
        if (command.CategoryId <= 0)
            throw new ValidationException(
                "شناسه دسته‌بندی معتبر نیست");

        var category = await _categoryRepository
            .GetByIdAsync(command.CategoryId);

        if (category is null)
            throw new NotFoundException(
                "دسته‌بندی مقاله موردنظر پیدا نشد");

        category.Activate();

        await _categoryRepository.UpdateAsync(category);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }
}