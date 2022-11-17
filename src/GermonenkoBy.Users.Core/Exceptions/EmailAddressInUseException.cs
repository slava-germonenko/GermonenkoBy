using GermonenkoBy.Common.Domain.Exceptions;

namespace GermonenkoBy.Users.Core.Exceptions;

public class EmailAddressInUseException : CoreLogicException
{
    public EmailAddressInUseException(string message) : base(message) { }
}