using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MusicEducation.Domain.Constants;
using MusicEducation.Domain.Entities.Commerce;
using MusicEducation.Domain.Entities.Courses;

namespace MusicEducation.Infrastructure.Persistence.Configurations.Commerce;

public class OrderItemConfiguration : IEntityTypeConfiguration<OrderItem>
{
    public void Configure(EntityTypeBuilder<OrderItem> builder)
    {
        builder.ToTable("OrderItems");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.OrderId)
            .IsRequired();

        builder.Property(x => x.CourseId)
            .IsRequired();

        builder.Property(x => x.CourseTitle)
            .HasMaxLength(DomainConstants.MaxOrderItemCourseTitleLength)
            .IsRequired();

        builder.OwnsOne(x => x.UnitPrice, money =>
        {
            money.Property(x => x.Amount)
                .HasPrecision(18, 2)
                .IsRequired();

            money.Property(x => x.Currency)
                .HasMaxLength(10)
                .IsRequired();
        });

        builder.HasOne<Order>()
            .WithMany()
            .HasForeignKey(x => x.OrderId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne<Course>()
            .WithMany()
            .HasForeignKey(x => x.CourseId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasIndex(x => new
        {
            x.OrderId,
            x.CourseId
        })
        .IsUnique();
    }
}