using Minimarket.DTO.Product;
using Minimarket.Entity;

namespace Minimarket.BLL.Products.Interfaces;

public interface IProductCategoryBLL
{
    Task<Result<ProductCategory>> Create(ProductCategoryDTO entity);

    Task<Result<ProductCategory>> Delete(int id);

    Task<Result<ProductCategory>> Edit(EditProductCategoryDTO entity);

    Task<Result<ResProductCategoryDTO>> GetCategory(int id);

    Task<Result<List<ResProductCategoryDTO>>> CategoryList(string seach);
}