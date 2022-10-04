namespace GermonenkoBy.Products.Core.Contracts.Repositories;

public interface IBulkMaterialsRepository
{
    public Task ReassignMaterialAsync(int materialId, int? newMaterialId);
}