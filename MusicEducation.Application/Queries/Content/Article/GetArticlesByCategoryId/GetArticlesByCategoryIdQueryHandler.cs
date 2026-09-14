using MusicEducation.Application.DTOs.Articles;
using MusicEducation.Application.Mappings.Articles;
using MusicEducation.Domain.Interfaces.Articles;

namespace MusicEducation.Application.Queries.Articles.Article;

public class GetArticlesByCategoryIdQueryHandler
{
    private readonly IArticleRepository _articleRepository;

    public GetArticlesByCategoryIdQueryHandler(IArticleRepository articleRepository)
    {
        _articleRepository = articleRepository;
    }

    public async Task<IEnumerable<ArticleDto>> Handle(GetArticlesByCategoryIdQuery query)
    {
        if (query.CategoryId <= 0)
            throw new ArgumentException("شناسه دسته‌بندی معتبر نیست",
                nameof(query.CategoryId));

        var articles = await _articleRepository
            .GetByCategoryIdAsync(query.CategoryId);

        return articles
            .Select(x => x.ToDto())
            .ToList();
    }
}