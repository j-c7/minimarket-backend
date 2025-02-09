using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Minimarket.BLL.Dashboard.Interfaces;

namespace Minimarket.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class DashboardController(IDashboardBLL dashBLL) : Controller
{
    [Authorize]
    [HttpGet("Summary")]
    public IActionResult Summary() =>
        Ok(dashBLL.Summary());
}