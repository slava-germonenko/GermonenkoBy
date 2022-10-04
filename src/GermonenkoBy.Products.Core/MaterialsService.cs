using Microsoft.EntityFrameworkCore;

using GermonenkoBy.Common.Domain.Exceptions;
using GermonenkoBy.Products.Core.Contracts;
using GermonenkoBy.Products.Core.Contracts.Repositories;
using GermonenkoBy.Products.Core.Dtos;
using GermonenkoBy.Products.Core.Models;

namespace GermonenkoBy.Products.Core;

public class MaterialsService
{
    private readonly ProductsContext _context;

    private readonly IBulkMaterialsRepository _bulkMaterialsRepository;

    public MaterialsService(
        ProductsContext context,
        IBulkMaterialsRepository bulkMaterialsRepository
    )
    {
        _context = context;
        _bulkMaterialsRepository = bulkMaterialsRepository;
    }

    public async Task<Material> GetMaterialAsync(int materialId)
    {
        var material = await _context.Materials.FindAsync(materialId);
        if (material is null)
        {
            throw new NotFoundException($"Материал с идентификатором {materialId} не найден.");
        }

        return material;
    }

    public async Task<Material> CreateMaterialAsync(SaveMaterialDto materialDto)
    {
        var materialNameIsInUse = await _context.Materials.AnyAsync(m => m.Name == materialDto.Name);
        if (materialNameIsInUse)
        {
            throw new CoreLogicException($"Название материала \"{materialDto.Name}\" уже используется.");
        }

        var material = new Material { Name = materialDto.Name };
        _context.Materials.Add(material);
        await _context.SaveChangesAsync();
        return material;
    }

    public async Task<Material> UpdateMaterialAsync(int materialId, SaveMaterialDto materialDto)
    {
        var materialNameIsInUse = await _context.Materials.AnyAsync(
            m => m.Name == materialDto.Name && m.Id != materialId
        );
        if (materialNameIsInUse)
        {
            throw new CoreLogicException($"Название материала \"{materialDto.Name}\" уже используется.");
        }

        var material = await _context.Materials.FindAsync(materialId);
        if (material is null)
        {
            throw new NotFoundException($"Материал с идентификатором {materialId} не найден.");
        }

        material.Name = materialDto.Name;

        _context.Materials.Update(material);
        await _context.SaveChangesAsync();
        return material;
    }

    public async Task RemoveMaterialAsync(int materialId)
    {
        var material = await _context.Materials.FindAsync(materialId);
        if (material is not null)
        {
            await _bulkMaterialsRepository.ReassignMaterialAsync(materialId, null);
            _context.Materials.Remove(material);
            await _context.SaveChangesAsync();
        }
    }
}