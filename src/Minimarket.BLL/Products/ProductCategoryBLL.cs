using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Minimarket.BLL.Products.Interfaces;
using Minimarket.DAL.Repository.Interfaces;
using Minimarket.DTO.Product;
using Minimarket.Entity;

namespace Minimarket.BLL.Products;

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

        return Result<ProductCategory>.Success(deletedCategory.Value!);
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

            return Result<ProductCategory>.Success(editedCategory.Value!);
        }
        return Result<ProductCategory>.Failure(["Categoria no encontrada"]);
    }

    public async Task<Result<ResProductCategoryDTO>> GetCategory(int id)
    {
        var category = await repo.Query(c => c.Id == id)
            //.Include(p => p.Products) 
            .FirstOrDefaultAsync();
        return category != null ? Result<ResProductCategoryDTO>
            .Success(mapper.Map<ResProductCategoryDTO>(category))
           : Result<ResProductCategoryDTO>.Failure(["Categoria no encontrada"]);
    }

    public async Task<Result<List<ResProductCategoryDTO>>> CategoryList(string seach)
    {
        seach = seach.ToLower();
        if (seach == "na" || seach == "all") seach = "";

        var categories = await repo.Query(c =>
        c.Name!
        .ToLower()
        .Contains(seach))
        //.Include(p => p.Products)
        .ToListAsync();

        if (categories == null || categories.Count == 0)
            return Result<List<ResProductCategoryDTO>>.Failure(["Sin resultados"]);

        List<ResProductCategoryDTO> resList = mapper.Map<List<ResProductCategoryDTO>>(categories);
        return Result<List<ResProductCategoryDTO>>.Success(resList);
    }
}