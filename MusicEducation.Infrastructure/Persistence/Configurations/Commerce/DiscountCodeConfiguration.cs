using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MusicEducation.Domain.Constants;
using MusicEducation.Domain.Entities.Commerce;

namespace MusicEducation.Infrastructure.Persistence.Configurations.Commerce;

public class DiscountCodeConfiguration : IEntityTypeConfiguration<DiscountCode>
{
    public void Configure(EntityTypeBuilder<DiscountCode> builder)
    {
        builder.ToTable("DiscountCodes");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Code)
            .HasMaxLength(DomainConstants.MaxDiscountCodeLength)
            .IsRequired();

        builder.HasIndex(x => x.Code)
            .IsUnique();

        builder.Property(x => x.Type)
            .HasConversion<int>()
            .IsRequired();

        builder.Property(x => x.Value)
            .HasPrecision(18, 2)
            .IsRequired();

        builder.OwnsOne(x => x.MinimumOrderAmount, money =>
        {
            money.Property(x => x.Amount)
                .HasPrecision(18, 2)
                .IsRequired();

            money.Property(x => x.Currency)
                .HasMaxLength(10)
                .IsRequired();
        });

        builder.Property(x => x.UsageLimit);

        builder.Property(x => x.UsedCount)
            .IsRequired();

        builder.OwnsOne(x => x.ValidityPeriod, validityPeriod =>
        {
            validityPeriod.Property(x => x.StartDate)
                .IsRequired();

            validityPeriod.Property(x => x.EndDate)
                .IsRequired();
        });

        builder.Property(x => x.IsActive)
            .IsRequired();
    }
}