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
    public async Task<Result<ProductCategory>> Create(ProductCategoryDTO entity)
    {
        var existCategory = await repo.Query(c => c.Name == entity.Name).AnyAsync();
        if (existCategory)
            return Result<ProductCategory>.Failure(["Esta categoria ya existe"]);

        var category = await repo.Create(mapper.Map<ProductCategory>(entity));
        if (!category.IsSucess)
            return Result<ProductCategory>.Failure(["Error al crear categoria"]);

        return Result<ProductCategory>
            .Success(category.Value!);
    }

    public async Task<Result<ProductCategory>> Delete(int id)
    {
        var category = await repo.Query(c => c.Id == id).FirstOrDefaultAsync();
        if (category == null)
            return Result<ProductCategory>.Failure(["La categoria no existe"]);

        var deletedCategory = await repo.Delete(category);

        if (!deletedCategory.IsSucess)
            return Result<ProductCategory>.Failure(["Imposible borrar categoria"]);

        return Result<ProductCategory>
            .Success(deletedCategory.Value!);
    }

    public async Task<Result<ProductCategory>> Edit(EditProductCategoryDTO entity)
    {
        var category = await repo.Query(c => c.Id == entity.Id).FirstOrDefaultAsync();
        if (category != null)
        {
            if (!string.IsNullOrEmpty(entity.Name))
                category.Name = entity.Name;

            var editedCategory = await repo.Edit(category);
            if (!editedCategory.IsSucess)
                return Result<ProductCategory>.Failure(editedCategory.Errors);

            return Result<ProductCategory>
                .Success(mapper.Map<ProductCategory>(editedCategory.Value));

        }
        return Result<ProductCategory>.Failure(["Categoria no encontrada"]);
    }

    public async Task<Result<ProductCategory>> GetCategory(int id)
    {
        var category = await repo.Query(c => c.Id == id).FirstOrDefaultAsync();
        return category != null ? Result<ProductCategory>.Success(category)
           : Result<ProductCategory>.Failure(["Categoria no encontrada"]);
    }

    public async Task<Result<List<ProductCategory>>> CategoryList(string seach)
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
            return Result<List<ProductCategory>>.Failure(["Sin resultados"]);
        }
        List<ProductCategory> resList = mapper.Map<List<ProductCategory>>(categories);
        return Result<List<ProductCategory>>.Success(resList);
    }
}