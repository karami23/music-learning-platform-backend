using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MusicEducation.Domain.Constants;
using MusicEducation.Domain.Entities.Commerce;
using MusicEducation.Domain.Entities.Identity;

namespace MusicEducation.Infrastructure.Persistence.Configurations.Commerce;

public class OrderConfiguration : IEntityTypeConfiguration<Order>
{
    public void Configure(EntityTypeBuilder<Order> builder)
    {
        builder.ToTable("Orders");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.UserId)
            .IsRequired();

        builder.HasOne<User>()
            .WithMany()
            .HasForeignKey(x => x.UserId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.Property(x => x.OrderNumber)
            .HasMaxLength(DomainConstants.MaxOrderNumberLength)
            .IsRequired();

        builder.HasIndex(x => x.OrderNumber)
            .IsUnique();

        builder.Property(x => x.Status)
            .HasConversion<int>()
            .IsRequired();

        builder.OwnsOne(x => x.Subtotal, money =>
        {
            money.Property(x => x.Amount)
                .HasPrecision(18, 2)
                .IsRequired();

            money.Property(x => x.Currency)
                .HasMaxLength(10)
                .IsRequired();
        });

        builder.OwnsOne(x => x.DiscountAmount, money =>
        {
            money.Property(x => x.Amount)
                .HasPrecision(18, 2)
                .IsRequired();

            money.Property(x => x.Currency)
                .HasMaxLength(10)
                .IsRequired();
        });

        builder.OwnsOne(x => x.TotalAmount, money =>
        {
            money.Property(x => x.Amount)
                .HasPrecision(18, 2)
                .IsRequired();

            money.Property(x => x.Currency)
                .HasMaxLength(10)
                .IsRequired();
        });

        builder.Property(x => x.DiscountCodeId);

        builder.HasOne<DiscountCode>()
            .WithMany()
            .HasForeignKey(x => x.DiscountCodeId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}