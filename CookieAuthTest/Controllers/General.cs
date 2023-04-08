using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace JwtTest_backend.Controllers;

[ApiController]
public class General : ControllerBase
{
    [HttpGet("GetMessage"), Authorize]
    public IActionResult GetMessage()
    {
        return Ok("Hello World");
    }
}
