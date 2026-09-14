using MusicEducation.Application.DTOs.Articles;
using MusicEducation.Application.Mappings.Articles;
using MusicEducation.Domain.Interfaces.Articles;

namespace MusicEducation.Application.Queries.Articles.Article;

public class GetArticleBySlugQueryHandler
{
    private readonly IArticleRepository _articleRepository;

    public GetArticleBySlugQueryHandler(IArticleRepository articleRepository)
    {
        _articleRepository = articleRepository;
    }

    public async Task<ArticleDto> Handle(GetArticleBySlugQuery query)
    {
        if (string.IsNullOrWhiteSpace(query.Slug))
            throw new ArgumentException("اسلاگ مقاله الزامی است",
                nameof(query.Slug));

        var article = await _articleRepository
            .GetBySlugAsync(query.Slug.Trim().ToLowerInvariant());

        if (article is null)
            throw new KeyNotFoundException("مقاله موردنظر پیدا نشد");

        return article.ToDto();
    }
}