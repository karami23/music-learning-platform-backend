using MusicEducation.Application.DTOs.Articles;
using MusicEducation.Application.Mappings.Articles;
using MusicEducation.Domain.Interfaces.Articles;

namespace MusicEducation.Application.Queries.Articles.Article;

public class GetArticleByIdQueryHandler
{
    private readonly IArticleRepository _articleRepository;

    public GetArticleByIdQueryHandler(IArticleRepository articleRepository)
    {
        _articleRepository = articleRepository;
    }

    public async Task<ArticleDto> Handle(GetArticleByIdQuery query)
    {
        if (query.ArticleId <= 0)
            throw new ArgumentException("شناسه مقاله معتبر نیست",
                nameof(query.ArticleId));

        var article = await _articleRepository
            .GetByIdAsync(query.ArticleId);

        if (article is null)
            throw new KeyNotFoundException("مقاله موردنظر پیدا نشد");

        return article.ToDto();
    }
}