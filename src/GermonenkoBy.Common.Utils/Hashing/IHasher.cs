namespace GermonenkoBy.Common.Utils.Hashing;

public interface IHasher
{
    public string GetHash(string source, string salt);

    public (string hash, string salt) GetHash(string source);
}