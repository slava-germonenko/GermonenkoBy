namespace GermonenkoBy.Common.Web.Middleware;

public enum ExceptionHandlingBehavior
{
    RethrowGenericError,
    RethrowDetailedError,
    NoCatch,
}