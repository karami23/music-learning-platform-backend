using MusicEducation.Domain.Exceptions;

namespace MusicEducation.Domain.Entities.Commerce;

public class Cart : BaseEntity
{
    public int UserId { get; private set; }

    private readonly List<CartItem> _items = new();

    public IReadOnlyCollection<CartItem> Items => _items.AsReadOnly();

    private Cart() { }

    public static Cart Create(int userId)
    {
        ValidateUserId(userId);

        return new Cart
        {
            UserId = userId
        };
    }

    public void AddItem(CartItem item)
    {
        ArgumentNullException.ThrowIfNull(item);

        if (_items.Any(x => x.CourseId == item.CourseId))
            throw new DomainException("این دوره قبلاً در سبد خرید قرار گرفته است", nameof(item));

        _items.Add(item);

        UpdatedAt = DateTime.UtcNow;
    }

    public void RemoveItem(int courseId)
    {
        if (courseId <= 0)
            throw new DomainException("شناسه دوره معتبر نیست", nameof(courseId));

        var item = _items.FirstOrDefault(x => x.CourseId == courseId);

        if (item is null)
            throw new DomainException("این دوره در سبد خرید وجود ندارد", nameof(courseId));

        _items.Remove(item);

        UpdatedAt = DateTime.UtcNow;
    }

    public void Clear()
    {
        _items.Clear();

        UpdatedAt = DateTime.UtcNow;
    }

    private static void ValidateUserId(int userId)
    {
        if (userId <= 0)
            throw new DomainException("شناسه کاربر معتبر نیست", nameof(userId));
    }
}