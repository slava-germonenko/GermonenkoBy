using GermonenkoBy.Products.Core.Dtos;
using GermonenkoBy.Products.Core.Models;

namespace GermonenkoBy.Products.Core.Extensions;

public static class ProductExtensions
{
    public static void CopyDetailsFrom(this Product product, ModifyProductDto productDto)
    {
        product.Active = productDto.Active;
        product.ItemNumber = productDto.ItemNumber;
        product.Name = productDto.Name;
        product.InternationalName = productDto.InternationalName;
    }
}