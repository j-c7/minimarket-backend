using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Minimarket.BLL.Products.Interfaces;
using Minimarket.DAL.Repository.Interfaces;
using Minimarket.DTO.Product;
using Minimarket.Entity;

namespace Minimarket.BLL.Products;

public class ProductBLL(IGenericRepo<Product> repo, IGenericRepo<ProductCategory> crepo, IMapper mapper) : IProductBLL
{
    public async Task<Result<Product>> Create(ProductDTO entity)
    {
        var checkCategory = await crepo.Query(c => c.Id == entity.CategoryId).FirstOrDefaultAsync();
        if (checkCategory == null)
            return Result<Product>.Failure([
                "La categoría no existe",
                "Cree una nueva categoría o verifique que introdujo el Id correcto!"
            ]);

        var existProduct = await repo.Query(p => p.Name == entity.Name).AnyAsync();
        if (existProduct)
            return Result<Product>.Failure(["Este producto ya existe"]);

        var product = await repo.Create(mapper.Map<Product>(entity));
        if (!product.IsSucess)
            return Result<Product>.Failure(["Error al crear producto"]);

        return Result<Product>.Success(product.Value!);
    }

    public async Task<Result<Product>> Delete(int id)
    {
        var product = await repo.Query(p => p.Id == id).FirstOrDefaultAsync();
        if (product == null)
            return Result<Product>.Failure(["El producto no existe"]);

        var deletedProduct = await repo.Delete(product);

        if (!deletedProduct.IsSucess)
            return Result<Product>.Failure(["Imposible borrar producto"]);

        return Result<Product>.Success(deletedProduct.Value!);
    }

    public async Task<Result<Product>> Edit(EditProductDTO entity)
    {
        if (entity.CategoryId is not null)
        {
            var checkCategory = await crepo.Query(c => c.Id == entity.CategoryId).AnyAsync();
            if (!checkCategory)
                return Result<Product>.Failure([
                    "La categoría no existe",
                    "Cree una nueva categoría o verifique que introdujo el Id correcto!"
                ]);
        }

        var product = await repo.Query(p => p.Id == entity.Id).FirstOrDefaultAsync();
        if (product != null)
        {
            if (entity.CategoryId is not null)
                product.CategoryId = entity.CategoryId;

            if (!string.IsNullOrEmpty(entity.Name))
                product.Name = entity.Name;

            if (entity.Price is not null)
                product.Price = entity.Price;

            if (entity.Stock is not null)
                product.Stock = entity.Stock;

            if (entity.IsActive is not null)
                product.IsActive = entity.IsActive;

            var editedProduct = await repo.Edit(product);
            if (!editedProduct.IsSucess)
                return Result<Product>.Failure(editedProduct.Errors);

            return Result<Product>.Success(editedProduct.Value!);
        }
        return Result<Product>.Failure(["Producto no encontrado"]);
    }

    public async Task<Result<ResProductDTO>> GetProduct(int id)
    {
        var product = await repo.Query(p => p.Id == id).FirstOrDefaultAsync();
        return product != null ? Result<ResProductDTO>.Success(mapper.Map<ResProductDTO>(product))
           : Result<ResProductDTO>.Failure(["Producto no encontrado"]);
    }

    /// Obtiene todos los productos sin importar la categoría.
    public async Task<Result<List<ResProductDTO>>> ProductList(string category, string seach)
    {
        category = category.ToLower();
        seach = seach.ToLower();
        if (category == "na" || category == "all") category = "";
        if (seach == "na" || seach == "all") seach = "";

        async Task<List<Product>> GetEntities(bool useCategory)
        {
            return await repo.Query(prod =>
            (useCategory != true || (prod.Category!.Name == category)) &&
            prod.Name!
            .ToLower()
            .Contains(seach))
            .ToListAsync();
        }
        var products = category != "" ? await GetEntities(true) : await GetEntities(false);
        if (products == null || products.Count == 0)
        {
            return Result<List<ResProductDTO>>.Failure(["Sin resultados"]);
        }
        List<ResProductDTO> resList = mapper.Map<List<ResProductDTO>>(products);
        return Result<List<ResProductDTO>>.Success(resList);
    }
}
