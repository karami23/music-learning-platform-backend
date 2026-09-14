using MusicEducation.Domain.Exceptions;

namespace MusicEducation.Domain.ValueObjects;

public sealed record class DateRange
{
    public DateTime StartDate { get; }

    public DateTime EndDate { get; }

    private DateRange(
        DateTime startDate,
        DateTime endDate)
    {
        StartDate = startDate;
        EndDate = endDate;
    }

    public static DateRange Create(
        DateTime startDate,
        DateTime endDate)
    {
        if (startDate > endDate)
            throw new DomainException("تاریخ شروع نمی‌تواند بعد از تاریخ پایان باشد", nameof(endDate));

        return new DateRange(
            startDate,
            endDate);
    }

    public bool Contains(DateTime date)
    {
        return date >= StartDate && date <= EndDate;
    }

    public TimeSpan Duration => EndDate - StartDate;

    public override string ToString()
    {
        return $"{StartDate:yyyy/MM/dd} تا {EndDate:yyyy/MM/dd}";
    }
}