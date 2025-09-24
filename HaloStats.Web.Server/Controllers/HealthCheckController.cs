using Microsoft.AspNetCore.Mvc;

namespace HaloStats.Web.Server.Controllers;

[ApiController]
[Route("api/[controller]")]
public class HealthCheckController : ControllerBase
{
    [HttpGet]
    public IActionResult Get()
    {
        return Ok("Healthy");
    }

    [HttpPost]
    public IActionResult TestPost([FromBody] object obj)
    {
        return Ok("Reached");
    }
}
