namespace MusicEducation.Domain.Exceptions;

public class DomainException : Exception
{
    public string? PropertyName { get; }

    public DomainException(string message, string? propertyName = null): base(message)
    {
        PropertyName = propertyName;
    }
}