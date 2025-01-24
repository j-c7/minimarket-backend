using Minimarket.DTO.Product;
using Minimarket.Entity;

namespace Minimarket.BLL.Product.Interfaces;

public interface IProductCategoryBLL
{
    Task<Result<ProductCategory>> Create(ProductCategoryDTO entity);

    Task<Result<ProductCategory>> Delete(int id);

    Task<Result<ProductCategory>> Edit(EditProductCategoryDTO entity);

    Task<Result<ProductCategory>> GetCategory(int id);

    Task<Result<List<ProductCategory>>> CategoryList(string seach);
}