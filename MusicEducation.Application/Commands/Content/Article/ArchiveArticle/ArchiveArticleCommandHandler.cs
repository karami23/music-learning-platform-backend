using MusicEducation.Application.Exceptions;
using MusicEducation.Application.Interfaces;
using MusicEducation.Domain.Interfaces.Articles;

namespace MusicEducation.Application.Commands.Articles.Article;

public sealed class ArchiveArticleCommandHandler
{
    private readonly IArticleRepository _articleRepository;
    private readonly IUnitOfWork _unitOfWork;

    public ArchiveArticleCommandHandler(
        IArticleRepository articleRepository,
        IUnitOfWork unitOfWork)
    {
        _articleRepository = articleRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task Handle(
        ArchiveArticleCommand command,
        CancellationToken cancellationToken = default)
    {
        if (command.ArticleId <= 0)
            throw new ValidationException("شناسه مقاله معتبر نیست");

        var article = await _articleRepository
            .GetByIdAsync(command.ArticleId);

        if (article is null)
            throw new NotFoundException("مقاله موردنظر پیدا نشد");

        article.Archive();

        await _articleRepository.UpdateAsync(article);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }
}