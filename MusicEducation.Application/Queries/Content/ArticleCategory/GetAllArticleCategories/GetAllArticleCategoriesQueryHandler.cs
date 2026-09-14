using MusicEducation.Application.DTOs.Articles;
using MusicEducation.Application.Mappings.Articles;
using MusicEducation.Domain.Interfaces.Articles;

namespace MusicEducation.Application.Queries.Articles.ArticleCategory;

public class GetAllArticleCategoriesQueryHandler
{
    private readonly IArticleCategoryRepository _categoryRepository;

    public GetAllArticleCategoriesQueryHandler(IArticleCategoryRepository categoryRepository)
    {
        _categoryRepository = categoryRepository;
    }

    public async Task<IEnumerable<ArticleCategoryDto>> Handle(GetAllArticleCategoriesQuery query)
    {
        var categories = await _categoryRepository.GetAllAsync();

        return categories
            .Select(x => x.ToDto())
            .ToList();
    }
}