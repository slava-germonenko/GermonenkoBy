using Azure.Storage.Blobs;

using GermonenkoBy.Products.Core.Contracts.Clients;

namespace GermonenkoBy.Products.Infrastructure.Clients;

public class AzureAssetsBlobClient : IAssetsBlobClient
{
    private const string ContainerName = "assets";

    private readonly BlobServiceClient _blobServiceClient;

    public AzureAssetsBlobClient(BlobServiceClient blobServiceClient)
    {
        _blobServiceClient = blobServiceClient;
    }

    public async Task<Uri> UploadAssetAsync(string fileName, byte[] content)
    {
        var containerClient = CreateContainerClient();
        var blobClient = containerClient.GetBlobClient(fileName);
        await blobClient.UploadAsync(new BinaryData(content));
        return blobClient.Uri;
    }

    public async Task SetAssetPropertyAsync(string fileName, string key, string? value)
    {
        var containerClient = CreateContainerClient();
        var blobClient = containerClient.GetBlobClient(fileName);
        var metadata = (await blobClient.GetPropertiesAsync()).Value.Metadata;

        metadata.Remove(key);
        if (value is not null)
        {
            metadata.Add(key, value);
        }

        await blobClient.SetMetadataAsync(metadata);
    }

    public Task DeleteAssetAsync(string fileName)
    {
        var containerClient = CreateContainerClient();
        var blobClient = containerClient.GetBlobClient(fileName);
        return blobClient.DeleteAsync();
    }

    private BlobContainerClient CreateContainerClient() => _blobServiceClient.GetBlobContainerClient(ContainerName);
}