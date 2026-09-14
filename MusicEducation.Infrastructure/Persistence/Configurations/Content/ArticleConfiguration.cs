using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MusicEducation.Domain.Constants;
using MusicEducation.Domain.Entities.Articles;

namespace MusicEducation.Infrastructure.Persistence.Configurations.Content;

public class ArticleConfiguration : IEntityTypeConfiguration<Article>
{
    public void Configure(EntityTypeBuilder<Article> builder)
    {
        builder.ToTable("Articles");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.ArticleCategoryId)
            .IsRequired();

        builder.Property(x => x.Title)
            .HasMaxLength(DomainConstants.MaxArticleTitleLength)
            .IsRequired();

        builder.Property(x => x.Slug)
            .HasMaxLength(DomainConstants.MaxArticleSlugLength)
            .IsRequired();

        builder.HasIndex(x => x.Slug)
            .IsUnique();

        builder.Property(x => x.Summary)
            .HasMaxLength(DomainConstants.MaxArticleSummaryLength);

        builder.Property(x => x.Content)
            .IsRequired();

        builder.Property(x => x.CoverImageUrl)
            .HasMaxLength(DomainConstants.MaxArticleCoverImageUrlLength);

        builder.Property(x => x.MetaTitle)
            .HasMaxLength(DomainConstants.MaxArticleMetaTitleLength);

        builder.Property(x => x.MetaDescription)
            .HasMaxLength(DomainConstants.MaxArticleMetaDescriptionLength);

        builder.Property(x => x.Status)
            .HasConversion<int>()
            .IsRequired();

        builder.Property(x => x.PublishedAt);

        builder.HasOne<ArticleCategory>()
            .WithMany(x => x.Articles)
            .HasForeignKey(x => x.ArticleCategoryId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasIndex(x => x.ArticleCategoryId);
    }
}
