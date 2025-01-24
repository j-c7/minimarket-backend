using Microsoft.AspNetCore.Mvc;
using Minimarket.BLL.Product.Interfaces;
using Minimarket.DTO.Product;

namespace Minimarket.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProductCategoryController(IProductCategoryBLL productCategoryBLL) : Controller
{
    [HttpPost("Create")]
    public async Task<IActionResult> Create([FromBody] ProductCategoryDTO entity) =>
        Ok(await productCategoryBLL.Create(entity));

    [HttpDelete("Delete/{id:int}")]
    public async Task<IActionResult> Delete(int id) =>
        Ok(await productCategoryBLL.Delete(id));

    [HttpPatch("Edit")]
    public async Task<IActionResult> Edit([FromBody] EditProductCategoryDTO entity) =>
        Ok(await productCategoryBLL.Edit(entity));

    [HttpGet("GetCategory/{id:int}")]
    public async Task<IActionResult> GetCategory(int id) =>
        Ok(await productCategoryBLL.GetCategory(id));

    [HttpGet("CategoryList/{seach:alpha?}")]
    public async Task<IActionResult> CategoryList(string seach) =>
        Ok(await productCategoryBLL.CategoryList(seach));

}