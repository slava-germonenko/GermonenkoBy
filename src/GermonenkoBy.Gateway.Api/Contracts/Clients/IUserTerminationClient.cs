namespace GermonenkoBy.Gateway.Api.Contracts.Clients;

public interface IUserTerminationClient
{
    public Task TerminateAsync(int userId);
}