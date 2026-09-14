using MusicEducation.Application.DTOs.Articles;
using MusicEducation.Application.Mappings.Articles;
using MusicEducation.Domain.Interfaces.Articles;

namespace MusicEducation.Application.Queries.Articles.ArticleCategory;

public class GetArticleCategoryByIdQueryHandler
{
    private readonly IArticleCategoryRepository _categoryRepository;

    public GetArticleCategoryByIdQueryHandler(IArticleCategoryRepository categoryRepository)
    {
        _categoryRepository = categoryRepository;
    }

    public async Task<ArticleCategoryDto> Handle(GetArticleCategoryByIdQuery query)
    {
        if (query.CategoryId <= 0)
            throw new ArgumentException("شناسه دسته‌بندی معتبر نیست",
                nameof(query.CategoryId));

        var category = await _categoryRepository.GetByIdAsync(query.CategoryId);

        if (category is null)
            throw new KeyNotFoundException("دسته‌بندی مقاله موردنظر پیدا نشد");

        return category.ToDto();
    }
}