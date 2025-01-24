using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;
using Minimarket.BLL.Product.Interfaces;
using Minimarket.DAL.Repository.Interfaces;
using Minimarket.DTO.Product;
using Minimarket.Entity;

namespace Minimarket.BLL.Product;

public class ProductCategoryBLL(IGenericRepo<ProductCategory> repo, IMapper mapper) : IProductCategoryBLL
{
    public async Task<Result<ProductCategoryResponseDTO>> Create(ProductCategoryDTO entity)
    {
        var existCategory = await repo.Query(c => c.Name == entity.Name).AnyAsync();
        if (existCategory)
            return Result<ProductCategoryResponseDTO>.Failure(["Esta categoria ya existe"]);

        var category = await repo.Create(mapper.Map<ProductCategory>(entity));
        if (!category.IsSucess)
            return Result<ProductCategoryResponseDTO>.Failure(["Error al crear categoria"]);

        return Result<ProductCategoryResponseDTO>
            .Success(mapper.Map<ProductCategoryResponseDTO>(category.Value));
    }

    public async Task<Result<ProductCategoryResponseDTO>> Delete(int id)
    {
        var category = await repo.Query(c => c.Id == id).FirstOrDefaultAsync();
        if (category == null)
            return Result<ProductCategoryResponseDTO>.Failure(["La categoria no existe"]);

        var deletedCategory = await repo.Delete(category);

        if (!deletedCategory.IsSucess)
            return Result<ProductCategoryResponseDTO>.Failure(["Imposible borrar categoria"]);

        return Result<ProductCategoryResponseDTO>
            .Success(mapper.Map<ProductCategoryResponseDTO>(deletedCategory));
    }

    public async Task<Result<ProductCategoryResponseDTO>> Edit(EditProductCategoryDTO entity)
    {
        var category = await repo.Query(c => c.Id == entity.Id).FirstOrDefaultAsync();
        if (category != null)
        {
            if (!string.IsNullOrEmpty(entity.Name))
                category.Name = entity.Name;

            var editedCategory = await repo.Edit(category);
            if (!editedCategory.IsSucess)
                return Result<ProductCategoryResponseDTO>.Failure(editedCategory.Errors);

            return Result<ProductCategoryResponseDTO>
                .Success(mapper.Map<ProductCategoryResponseDTO>(editedCategory.Value));

        }
        return Result<ProductCategoryResponseDTO>.Failure(["Categoria no encontrada"]);
    }

    public async Task<Result<ProductCategoryResponseDTO>> GetCategory(int id)
    {
        var category = await repo.Query(c => c.Id == id).FirstOrDefaultAsync();
        return category != null ? Result<ProductCategoryResponseDTO>.Success(mapper.Map<ProductCategoryResponseDTO>(category))
           : Result<ProductCategoryResponseDTO>.Failure(["Categoria no encontrada"]);
    }

    public async Task<Result<List<ProductCategoryResponseDTO>>> CategoryList(string seach)
    {
        seach = seach.ToLower();
        if (seach == "na" || seach == "all") seach = "";
        
        var categories = await repo.Query(c => 
        c.Name!
        .ToLower()
        .Contains(seach))
        .ToListAsync();
        if (categories == null || categories.Count == 0)
        {
            return Result<List<ProductCategoryResponseDTO>>.Failure(["Sin resultados"]);
        }
        List<ProductCategoryResponseDTO> resList = mapper.Map<List<ProductCategoryResponseDTO>>(categories);
        return Result<List<ProductCategoryResponseDTO>>.Success(resList);
    }
}