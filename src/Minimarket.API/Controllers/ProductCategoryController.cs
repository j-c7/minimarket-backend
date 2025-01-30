using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Minimarket.BLL.Products.Interfaces;
using Minimarket.DTO.Product;

namespace Minimarket.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProductCategoryController(IProductCategoryBLL productCategoryBLL) : Controller
{
    [Authorize]
    [HttpPost("Create")]
    public async Task<IActionResult> Create([FromBody] ProductCategoryDTO entity) =>
        Ok(await productCategoryBLL.Create(entity));

    [Authorize]
    [HttpDelete("Delete/{id:int}")]
    public async Task<IActionResult> Delete(int id) =>
        Ok(await productCategoryBLL.Delete(id));

    [Authorize]
    [HttpPatch("Edit")]
    public async Task<IActionResult> Edit([FromBody] EditProductCategoryDTO entity) =>
        Ok(await productCategoryBLL.Edit(entity));

    [Authorize]
    [HttpGet("GetCategory/{id:int}")]
    public async Task<IActionResult> GetCategory(int id) =>
        Ok(await productCategoryBLL.GetCategory(id));

    [Authorize]
    [HttpGet("CategoryList/{seach:alpha?}")]
    public async Task<IActionResult> CategoryList(string seach) =>
        Ok(await productCategoryBLL.CategoryList(seach));
}