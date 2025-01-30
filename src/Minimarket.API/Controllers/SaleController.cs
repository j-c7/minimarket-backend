using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Minimarket.BLL.Sales.Interfaces;
using Minimarket.DTO.Sale;

namespace Minimarket.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class SaleController(ISaleBLL saleBLL) : Controller
{
    [Authorize]
    [HttpPost("Register")]
    public async Task<IActionResult> Register([FromBody] SaleDTO entity) =>
        Ok(await saleBLL.Register(entity));

    [Authorize]
    [HttpGet("SaleHistory")]
    public async Task<IActionResult> SaleHistory(string seach, string saleNumber = "NA", string startDate = "NA", string endDate = "NA") =>
        Ok(await saleBLL.History(seach, saleNumber, startDate, endDate));

    [Authorize]
    [HttpGet("Report")]
    public async Task<IActionResult> Report(string startDate, string endDate) =>
        Ok(await saleBLL.Report(startDate, endDate));
}