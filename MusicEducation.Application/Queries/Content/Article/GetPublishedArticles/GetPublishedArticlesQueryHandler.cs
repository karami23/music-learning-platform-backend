using MusicEducation.Application.DTOs.Articles;
using MusicEducation.Application.Mappings.Articles;
using MusicEducation.Domain.Interfaces.Articles;

namespace MusicEducation.Application.Queries.Articles.Article;

public class GetPublishedArticlesQueryHandler
{
    private readonly IArticleRepository _articleRepository;

    public GetPublishedArticlesQueryHandler(IArticleRepository articleRepository)
    {
        _articleRepository = articleRepository;
    }

    public async Task<IEnumerable<ArticleDto>> Handle(GetPublishedArticlesQuery query)
    {
        var articles = await _articleRepository
            .GetPublishedAsync();

        return articles
            .Select(x => x.ToDto())
            .ToList();
    }
}