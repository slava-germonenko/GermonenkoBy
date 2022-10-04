namespace GermonenkoBy.Products.Core.Contracts;

public interface IBulkMaterialsRepository
{
    public Task ReassignMaterialAsync(int materialId, int? newMaterialId);
}