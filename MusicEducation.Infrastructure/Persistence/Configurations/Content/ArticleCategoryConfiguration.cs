using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MusicEducation.Domain.Constants;
using MusicEducation.Domain.Entities.Articles;

namespace MusicEducation.Infrastructure.Persistence.Configurations.Content;

public class ArticleCategoryConfiguration : IEntityTypeConfiguration<ArticleCategory>
{
    public void Configure(EntityTypeBuilder<ArticleCategory> builder)
    {
        builder.ToTable("ArticleCategories");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Name)
            .HasMaxLength(DomainConstants.MaxArticleCategoryNameLength)
            .IsRequired();

        builder.HasIndex(x => x.Name)
            .IsUnique();

        builder.Property(x => x.Description)
            .HasMaxLength(DomainConstants.MaxArticleCategoryDescriptionLength);

        builder.Property(x => x.IsActive)
            .IsRequired();
    }
}