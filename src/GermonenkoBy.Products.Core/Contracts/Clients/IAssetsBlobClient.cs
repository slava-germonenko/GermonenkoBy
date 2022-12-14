namespace GermonenkoBy.Products.Core.Contracts.Clients;

public interface IAssetsBlobClient
{
    public Task<Uri> UploadAssetAsync(string fileName, byte[] content, string? contentType = null);

    public Task SetAssetPropertyAsync(string fileName, string key, string? value);

    public Task DeleteAssetAsync(string fileName);
}