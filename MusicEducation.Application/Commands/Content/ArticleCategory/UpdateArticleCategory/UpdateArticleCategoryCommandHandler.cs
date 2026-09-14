using MusicEducation.Application.Exceptions;
using MusicEducation.Application.Interfaces;
using MusicEducation.Domain.Interfaces.Articles;

namespace MusicEducation.Application.Commands.Articles.ArticleCategory;

public sealed class UpdateArticleCategoryCommandHandler
{
    private readonly IArticleCategoryRepository _categoryRepository;
    private readonly IUnitOfWork _unitOfWork;

    public UpdateArticleCategoryCommandHandler(
        IArticleCategoryRepository categoryRepository,
        IUnitOfWork unitOfWork)
    {
        _categoryRepository = categoryRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task Handle(
        UpdateArticleCategoryCommand command,
        CancellationToken cancellationToken = default)
    {
        if (command.Id <= 0)
            throw new ValidationException(
                "شناسه دسته‌بندی معتبر نیست");

        if (string.IsNullOrWhiteSpace(command.Name))
            throw new ValidationException(
                "نام دسته‌بندی مقاله الزامی است");

        var category = await _categoryRepository
            .GetByIdAsync(command.Id);

        if (category is null)
            throw new NotFoundException(
                "دسته‌بندی مقاله موردنظر پیدا نشد");

        var normalizedName = command.Name.Trim();

        var categories = await _categoryRepository
            .GetAllAsync();

        var duplicate = categories.Any(x =>
            x.Id != category.Id &&
            string.Equals(
                x.Name,
                normalizedName,
                StringComparison.OrdinalIgnoreCase));

        if (duplicate)
            throw new ConflictException(
                "دسته‌بندی مقاله با این نام قبلاً وجود دارد");

        category.Update(
            normalizedName,
            command.Description);

        await _categoryRepository.UpdateAsync(category);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }
}