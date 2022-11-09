using GermonenkoBy.Common.Domain;
using GermonenkoBy.Gateway.Api.Models.Products;

namespace GermonenkoBy.Gateway.Api.Contracts.Clients;

public interface IMaterialsClient
{
    public Task<PagedSet<Material>> GetMaterialsAsync(MaterialsFilterDto materialsFilter);

    public Task<Material?> GetMaterialAsync(int materialId);

    public Task<Material> CreateMaterialAsync(SaveMaterialDto materialDto);

    public Task<Material> UpdateMaterialAsync(int materialId, SaveMaterialDto materialDto);

    public Task DeleteMaterial(int materialId);
}