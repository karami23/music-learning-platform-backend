using MusicEducation.Application.DTOs.Articles;
using MusicEducation.Application.Mappings.Articles;
using MusicEducation.Domain.Interfaces.Articles;

namespace MusicEducation.Application.Queries.Articles.Article;

public class GetAllArticlesQueryHandler
{
    private readonly IArticleRepository _articleRepository;

    public GetAllArticlesQueryHandler(IArticleRepository articleRepository)
    {
        _articleRepository = articleRepository;
    }

    public async Task<IEnumerable<ArticleDto>> Handle(GetAllArticlesQuery query)
    {
        var articles = await _articleRepository.GetAllAsync();

        return articles
            .Select(x => x.ToDto())
            .ToList();
    }
}