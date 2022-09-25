namespace GermonenkoBy.Common.Domain.Exceptions;

public class NotFoundException : Exception
{
    public NotFoundException() { }

    public NotFoundException(
        string message,
        Exception? innerException = null
    ): base(message, innerException) { }
}