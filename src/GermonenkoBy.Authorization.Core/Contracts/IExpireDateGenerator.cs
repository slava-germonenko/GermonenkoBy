namespace GermonenkoBy.Authorization.Core.Contracts;

public interface IExpireDateGenerator
{
    public DateTime GenerateSessionExpireDate();
}