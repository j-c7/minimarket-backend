using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Minimarket.BLL.User.Interfaces;
using Minimarket.DTO.User;

namespace Minimarket.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UserController(IUserBLL userBLL) : Controller
{
    [HttpPost("Create")]
    public async Task<IActionResult> Create([FromBody] UserProfileDTO entity) =>
        Ok(await userBLL.Create(entity));

    [Authorize]
    [HttpDelete("Delete/{id:int}")]
    public async Task<IActionResult> Delete(int id) =>
        Ok(await userBLL.Delete(id));

    [Authorize]
    [HttpPatch("Edit")]
    public async Task<IActionResult> Edit([FromBody] EditUserProfileDTO entity) =>
        Ok(await userBLL.Edit(entity));

    [Authorize]
    [HttpGet("GetUser/{id:int}")]
    public async Task<IActionResult> GetUser(int id) =>
        Ok(await userBLL.GetUser(id));

    [Authorize]
    [HttpGet("UserList/{role:alpha}/{seach:alpha?}")]
    public async Task<IActionResult> UserList(string role, string seach) =>
        Ok(await userBLL.UserList(role, seach));

    [HttpPost("Login")]
    public async Task<IActionResult> Login([FromBody] LoginDTO entity) =>
        Ok(await userBLL.Auth(entity));

    [Authorize]
    [HttpGet("Profile")]
    public async Task<IActionResult> Profile()
    {
        var context = HttpContext;
        return Ok(await userBLL.Profile(context));
    }

    [Authorize]
    [RequestFormLimits(MultipartBodyLengthLimit = 2000000)]
    [HttpPost("UploadProfileImage")]
    public async Task<IActionResult> UploadProfileImage(IFormFile image)
    {
        var context = HttpContext;
        return Ok(
            await userBLL.UploadProfileImage(image, context)
        );
    }

    [Authorize]
    [HttpDelete("DeleteProfileImage")]
    public async Task<IActionResult> DeleteProfileImage()
    {
        var context = HttpContext;
        return Ok(
            await userBLL.DeleteProfileImage(context)
        );
    }
}