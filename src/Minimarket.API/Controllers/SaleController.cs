using Microsoft.AspNetCore.Mvc;
using Minimarket.BLL.Sales.Interfaces;
using Minimarket.DTO.Sale;

namespace Minimarket.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class SaleController(ISaleBLL saleBLL) : Controller
{
    [HttpPost]
    public async Task<IActionResult> Register([FromBody] SaleDTO entity) =>
        Ok(await saleBLL.Register(entity));

    [HttpGet]
    public async Task<IActionResult> SaleHistory(string seach, string saleNumber = "NA", string startDate = "NA", string endDate = "NA") =>
        Ok(await saleBLL.History(seach, saleNumber, startDate, endDate));

}