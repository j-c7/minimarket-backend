using Microsoft.AspNetCore.Mvc;
using Minimarket.BLL.Products.Interfaces;
using Minimarket.DTO.Product;

namespace Minimarket.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProductController(IProductBLL productBLL) : Controller
{
    [HttpPost("Create")]
    public async Task<IActionResult> Create([FromBody] ProductDTO entity) =>
        Ok(await productBLL.Create(entity));

    [HttpDelete("Delete/{id:int}")]
    public async Task<IActionResult> Delete(int id) =>
        Ok(await productBLL.Delete(id));

    [HttpPatch("Edit")]
    public async Task<IActionResult> Edit([FromBody] EditProductDTO entity) =>
        Ok(await productBLL.Edit(entity));

    [HttpGet("GetProduct/{id:int}")]
    public async Task<IActionResult> GetProduct(int id) =>
        Ok(await productBLL.GetProduct(id));

    [HttpGet("ProductList/{seach:alpha?}")]
    public async Task<IActionResult> ProductList(string seach) =>
        Ok(await productBLL.ProductList(seach));

}