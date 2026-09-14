using MusicEducation.Application.DTOs.Articles;
using MusicEducation.Application.Mappings.Articles;
using MusicEducation.Domain.Interfaces.Articles;

namespace MusicEducation.Application.Queries.Articles.ArticleCategory;

public class GetActiveArticleCategoriesQueryHandler
{
    private readonly IArticleCategoryRepository _categoryRepository;

    public GetActiveArticleCategoriesQueryHandler(IArticleCategoryRepository categoryRepository)
    {
        _categoryRepository = categoryRepository;
    }

    public async Task<IEnumerable<ArticleCategoryDto>> Handle(GetActiveArticleCategoriesQuery query)
    {
        var categories = await _categoryRepository.GetActiveAsync();

        return categories
            .Select(x => x.ToDto())
            .ToList();
    }
}