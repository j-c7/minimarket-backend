using Minimarket.DTO.Product;
using Minimarket.Entity;

namespace Minimarket.BLL.Product.Interfaces;

public interface IProductCategoryBLL
{
    Task<Result<ProductCategoryResponseDTO>> Create(ProductCategoryDTO entity);

    Task<Result<ProductCategoryResponseDTO>> Delete(int id);

    Task<Result<ProductCategoryResponseDTO>> Edit(EditProductCategoryDTO entity);

    Task<Result<ProductCategoryResponseDTO>> GetCategory(int id);

    Task<Result<List<ProductCategoryResponseDTO>>> CategoryList(string seach);
}