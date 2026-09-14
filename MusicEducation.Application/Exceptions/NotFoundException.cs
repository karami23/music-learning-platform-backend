namespace MusicEducation.Application.Exceptions;

public class NotFoundException : Exception
{
    public NotFoundException(string message)
        : base(message) { }
}