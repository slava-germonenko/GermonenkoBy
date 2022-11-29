namespace GermonenkoBy.Common.Domain.Exceptions;

public class InfrastructureException : Exception
{
    public InfrastructureException() { }

    public InfrastructureException(
        string message,
        Exception? innerException = null
    ): base(message, innerException) { }
}