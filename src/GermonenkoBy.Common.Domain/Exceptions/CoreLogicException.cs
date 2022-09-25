namespace GermonenkoBy.Common.Domain.Exceptions;

public class CoreLogicException : Exception
{
    public CoreLogicException() { }

    public CoreLogicException(
        string message,
        Exception? innerException = null
    ): base(message, innerException) { }
}