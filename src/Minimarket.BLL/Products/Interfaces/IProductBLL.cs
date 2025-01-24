using Minimarket.DTO.Product;
using Minimarket.Entity;

namespace Minimarket.BLL.Products.Interfaces;

public interface IProductBLL
{
    Task<Result<Product>> Create(ProductDTO entity);

    Task<Result<Product>> Delete(int id);

    Task<Result<Product>> Edit(EditProductDTO entity);

    Task<Result<ResProductDTO>> GetProduct(int id);

    Task<Result<List<ResProductDTO>>> ProductList(string category, string seach);
}