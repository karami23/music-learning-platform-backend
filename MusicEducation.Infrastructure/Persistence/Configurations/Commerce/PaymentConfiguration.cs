using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MusicEducation.Domain.Entities.Commerce;

namespace MusicEducation.Infrastructure.Persistence.Configurations.Commerce;

public class PaymentConfiguration : IEntityTypeConfiguration<Payment>
{
    public void Configure(EntityTypeBuilder<Payment> builder)
    {
        builder.ToTable("Payments");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.OrderId)
            .IsRequired();

        builder.OwnsOne(x => x.Amount, money =>
        {
            money.Property(x => x.Amount)
                .HasPrecision(18, 2)
                .IsRequired();

            money.Property(x => x.Currency)
                .HasMaxLength(10)
                .IsRequired();
        });

        builder.Property(x => x.Status)
            .HasConversion<int>()
            .IsRequired();

        builder.Property(x => x.TransactionId)
            .HasMaxLength(200);

        builder.HasIndex(x => x.TransactionId)
            .IsUnique()
            .HasFilter("[TransactionId] IS NOT NULL");

        builder.Property(x => x.ReferenceNumber)
            .HasMaxLength(200);

        builder.Property(x => x.PaidAt);

        builder.HasOne<Order>()
            .WithMany()
            .HasForeignKey(x => x.OrderId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasIndex(x => x.OrderId);
    }
}