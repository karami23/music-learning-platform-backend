using MusicEducation.Domain.Exceptions;

namespace MusicEducation.Domain.Entities.Commerce;

public class CartItem : BaseEntity
{
    public int CartId { get; private set; }

    public int CourseId { get; private set; }

    private CartItem() { }

    public static CartItem Create(
        int cartId,
        int courseId)
    {
        ValidateCartId(cartId);
        ValidateCourseId(courseId);

        return new CartItem
        {
            CartId = cartId,
            CourseId = courseId
        };
    }

    private static void ValidateCartId(int cartId)
    {
        if (cartId <= 0)
            throw new DomainException("شناسه سبد خرید معتبر نیست", nameof(cartId));
    }

    private static void ValidateCourseId(int courseId)
    {
        if (courseId <= 0)
            throw new DomainException("شناسه دوره معتبر نیست", nameof(courseId));
    }
}